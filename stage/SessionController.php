<?php

namespace App\Controller;

use App\Entity\Contenu;
use App\Entity\Idee;
use App\Entity\Participant;
use App\Entity\User;
use App\Form\AddFacilitateurType;
use App\Form\FacilitateurToAtelierType;
use DateInterval;
use DateTime;
use DateTimeZone;
use Sensio\Bundle\FrameworkExtraBundle\Configuration\Method;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\HttpFoundation\Request;
use App\Entity\Session;
use App\Entity\Seance;
use App\Entity\Atelier;


class SessionController extends AbstractController
{
    //permet de creer d'une nouvelle session
    public function createSession(Request $request)
    {
        //recupération des superglobals
        $post = $request->request->all();

        $em = $this->getDoctrine()->getManager();
        //recupération d'un administrateur connecter
        $admin = $this->getDoctrine()->getRepository(User::class)->findOneBy(['id' => $this->getUser()->getId()]);

        //création d'une nouvelle session
        $session = new Session();
        $session->setNom($post["nom"]);
        $session->setDateDebut(new DateTime($post["debut"]));
        $session->setDateFin(new DateTime($post["fin"]));
        $session->setAdmin($em->find(User::class, $admin->getId()));
        $session->setIsValidate(false);
        $em->persist($session);
        $em->flush();

        return $this->json("success");
    }

    /**
     * @Method({"POST"})
     * dans le cas ou un participant s'inscrit de lui même sur l'application
     */
    public function registerSession(Request $request,$id)
    {
        $em = $this->getDoctrine()->getManager();

        //recupération des superglobals
        $post = $request->request->all();

        //creation d'un nouveau participant
        $participant = new Participant();
        $participant->setNom($post['nom']);
        $participant->setPrenom($post['prenom']);
        $participant->setEmail($post['email']);
        $participant->setTel($post['tel']);
        $participant->setSession($em->find(Session::class, $id));
        $participant->setIsPresent(false);
        $em->persist($participant);
        $em->flush();

        return $this->json("success");
    }

    /**
     * @Method({"POST"})
     * dans le cas ou un participant s'inscrit en se présentant à la session
     */
    public function registerLiveSession(Request $request,$id)
    {
        $em = $this->getDoctrine()->getManager();

        //recupération des superglobals
        $post = $request->request->all();

        //creation d'un nouveau participant
        $participant = new Participant();
        $participant->setNom($post['nom']);
        $participant->setPrenom($post['prenom']);
        $participant->setEmail($post['email']);
        $participant->setTel($post['tel']);
        $participant->setSession($em->find(Session::class, $id));
        $participant->setIsPresent(true);
        $em->persist($participant);
        $em->flush();

        //recuperation de tout les participant à une session
        //necessaire pour reconstruir les cartes en jquery
        $participants = $this->getDoctrine()->getRepository(Participant::class)->findBy(["session"=>$id], ["prenom" => "ASC"]);

        //initialisation d'un tableau de participant à retourner en json
        $participantArray = [];
        foreach ($participants as $participantSession){
            //inserer les participants dans le tableau
            array_push($participantArray, array(
                "id" => $participantSession->getId(),
                "idSession" => $participantSession->getSession()->getId(),
                "nom" => $participantSession->getNom(),
                "prenom" => $participantSession->getPrenom(),
                "isPresent" => $participantSession->getIsPresent()
            ));
        }
        return $this->json($participantArray);
    }

    public function validateParticipant($idP, $idS)
    {
        $em = $this->getDoctrine()->getManager();

        //recuperation du participant
        $participant = $this->getDoctrine()->getRepository(Participant::class)->find($idP);
        $participant->setIsPresent(true);
        $em->merge($participant);
        $em->flush();

        //recuperation de tout les participant à une session
        //necessaire pour reconstruir les cartes en jquery
        $participants = $this->getDoctrine()->getRepository(Participant::class)->findBy(["session"=>$idS], ["prenom" => "ASC"]);

        //initialisation d'un tableau de participant à retourner en json
        $participantArray = [];
        foreach ($participants as $participantSession){
            //inserer les participants dans le tableau
            array_push($participantArray, array(
                "id" => $participantSession->getId(),
                "idSession" => $participantSession->getSession()->getId(),
                "nom" => $participantSession->getNom(),
                "prenom" => $participantSession->getPrenom(),
                "isPresent" => $participantSession->getIsPresent()
            ));
        }
        return $this->json($participantArray);
    }

    //affichage de la vue d'une session à valider, à venir, et en cours
    public function viewSession(Request $request, $id)
    {
        //récupération des ateliers
        $ateliers = $this->getDoctrine()->getRepository(Atelier::class)->findBy(['session' => $id]);
        //récupération des participants classés par ordre alphabétique
        $participant = $this->getDoctrine()->getRepository(Participant::class)->findBy(['session' => $id], ['prenom' => 'ASC']);

        $em = $this->getDoctrine()->getManager();
        //recuperation de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);

        //creation du formulaire permettant d'ajouter et de supprimer à la session,
        //les facilitateurs qui ont été ajouter par un administrateur. ( fonction du filtre dans le UserRepository)
        $formF = $this->createForm(AddFacilitateurType::class, $session);
        $formF->handleRequest($request);

        if($formF->isSubmitted() && $formF->isValid()) {
            $em->merge($session);
            $em->flush();

            return $this->redirectToRoute('session', array('id' => $session->getId()));
        }

        return $this->render('session/index.html.twig', [
            'session' => $session,
            'ateliers' => $ateliers,
            'participantSession' => $participant,
            'formAddF' => $formF->createView(),
        ]);
    }

    //affichage de la vue d'une session terminer
    public function recapSession($id)
    {
        //recupération de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);
        //recupération des seances classées par date (fonction dans le SeanceRepository)
        $seances = $this->getDoctrine()->getRepository(Seance::class)->findAllByDate();
        //recupération des participants classés par ordre alphabétique
        $participant = $this->getDoctrine()->getRepository(Participant::class)->findBy(['session' => $id], ['prenom' => 'ASC']);
        //recupération des idées
        $idees = $this->getDoctrine()->getRepository(Idee::class)->findAll();

        return $this->render('session/recap.html.twig', [
            'session' => $session,
            'seances' => $seances,
            'participantSession' => $participant,
            'idees' => $idees
        ]);
    }

    /**
     * @Method({"POST"})
     * permet de creer un nouvel atelier
     */
    public function createAtelier(Request $request, $id)
    {
        $em = $this->getDoctrine()->getManager();

        //recupération des superglobals
        $post = $request->request->all();

        //création d'un nouvel atelier
        $atelier = new Atelier();
        $atelier->setNom($post['nom']);
        $atelier->setDescription($post['description']);
        $atelier->setSession($em->find(Session::class, $id));
        $em->persist($atelier);
        $em->flush();

        return $this->json(array("success"));
    }

    /**
     * @Method({"POST"})
     * permet de creer une nouvelle seance (multifom.js)
     */
    public function createSeance(Request $request, $id)
    {
        $em = $this->getDoctrine()->getManager();

        //recuperation de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);

        //recupération de la superglobal $_POST
        $post = $request->request->all();

        //recupéréation du jour, heure de début et heure de fin pour la création d'une date compléte
        $jour = $post['date'];
        $heureDebut = $post['heure_debut'];
        $heureFin = $post['heure_fin'];

        //création d'une séance
        $seance = new Seance();
        $seance->setAdresse($post['adresse']);
        $seance->setDate(new DateTime($jour));
        $seance->setHeureDebut(new DateTime("$jour $heureDebut"));
        $seance->setHeureFin(new DateTime("$jour $heureFin"));
        $seance->setAtelier($em->find(Atelier::class, $post['atelier']));
        $em->persist($seance);
        $em->flush();

        //recuperation des ateliers
        $ateliers = $this->getDoctrine()->getRepository(Atelier::class)->findBy(['session'=>$id]);

        $sessionArray = [];
        //stock dans un tableau la date de début et la date de fin d'une session
        // afin de rendre inutilisable les dates en dehors de cette tranche
        array_push($sessionArray, array(
            "date_debut" => $session->getDateDebut()->format("Y-m-d"),
            "date_fin" => $session->getDateFin()->format("Y-m-d")
        ));

        $ateliersArray = [];
        foreach ($ateliers as $atelier){
            //stock dans un tableau l'id et le nom des ateliers afin de les afficher dynamiquement dans le select
            array_push($ateliersArray, array(
                "id" => $atelier->getId(),
                "nom" => $atelier->getNom()
            ));
        }

        return $this->json(array(
            $sessionArray,
            $ateliersArray
        ));
    }

    /**
     * @Method({"POST"})
     * permet de modifier les valeurs des propriété d'une seance et d'un atelier
     */
    public function updateSeance(Request $request, $id){
        $em = $this->getDoctrine()->getManager();
        //recupération des superglobal
        $post = $request->request->all();

        //recuperation de la seance
        $seance = $this->getDoctrine()->getRepository(Seance::class)->find($id);
        //recuperation de la date et l'heure de debut des seances
        $heureDebut = $seance->getHeureDebut();
        //recuperation du jour
        $jour = $heureDebut->format('Y-m-d');

        //recuperation du post des heures modifiees
        $newHeureDebut = new DateTime($post['heure_debut']);
        $newHeureFin = new DateTime($post['heure_fin']);
        //transformation des posts au format heure/minutes
        $heureDebutToSet = $newHeureDebut->format("H:i");
        $heureFinToSet = $newHeureFin->format("H:i");

        //recupération de l'atelier de la seance
        $atelier = $seance->getAtelier();

        //apport des modification au propriété de l'atelier
        $atelier->setNom($post['nom']);
        $atelier->setDescription($post['description']);
        $em->merge($atelier);

        //apport des modification au propriété de la seance
        $seance->setAdresse($post['adresse']);
        $seance->setHeureDebut(new DateTime("$jour $heureDebutToSet"));
        $seance->setHeureFin(new DateTime("$jour $heureFinToSet"));
        $em->merge($seance);

        $em->flush();

        return $this->json("success");
    }

    /**
     * @Method({"POST"})
     * permet de valider un session (datatable)
     */
    public function validateSession($id){
        $em = $this->getDoctrine()->getManager();
        //recuperation de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);

        //mise a jour du booleen en base de données
        $session->setIsValidate(true);
        $em->merge($session);
        $em->flush();

        return $this->json(array("success"));
    }

    //permet de modifier les propiétés d'une session
    public function updateSession(Request $request, $id){
        $em = $this->getDoctrine()->getManager();
        //recuperation de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);

        //recupération de la superglobal $_POST
        $post = $request->request->all();

        //apport des modification au propriété de la session
        $session->setNom($post['nomm']);
        $session->setDateDebut(new DateTime($post['date_debut']));
        $session->setDateFin(new DateTime($post['date_fin']));

        $em->merge($session);
        $em->flush();

        return $this->json("success");
    }

    /**
     * @Method({"POST"})
     * permet supprimer un facilitateur (datatable)
     */
    public function deleteFacilitateur($idFacilitateur){
        $em = $this->getDoctrine()->getManager();
        //delete facilitateur
        $facilitateur = $this->getDoctrine()->getRepository(User::class)->find($idFacilitateur);
        $em->remove($facilitateur);
        $em->flush();

        return $this->json(array("success"));
    }

    /**
     * @Method({"POST"})
     * permet d'annuler une session (datatable)
     */
    public function removeSession($id){
        $em = $this->getDoctrine()->getManager();
        //recuperation de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);

        //mise a jour du booleen en base de données
        $session->setIsValidate(false);
        $em->merge($session);
        $em->flush();

        return $this->json(array("success"));
    }

    /**
     * @Method({"POST"})
     * permet d'ajouter un facilitateur à un ou plusieurs atelier
     */
    public function addFacilitateurToAtelier(Request $request, $id){
        $em = $this->getDoctrine()->getManager();
        $post = $request->request->all();

        //recuperation des facilitateurs de la session
        $facilitateur = $this->getDoctrine()->getRepository(User::class)->find($id);
        //récuperation des ateliers selectionnés
        $ateliers = $this->getDoctrine()->getRepository(Atelier::class)->findBy(['id'=>$post['ateliers']]);


        //Assignation des ateliers a un facilitateur
        foreach($ateliers as $value){
            $value->addFacilitateur($em->find(User::class, $facilitateur->getId()));
        }

        $em->flush();

        return $this->json(array("success"));
    }

    /**
     * @Method({"GET"})
     * permet de récupérer les valeurs en json nécessaire à l'utilisation des events dans calendar
     */
    public function getCalendarSession($id){
        //recuperation de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);
        //recuperation des ateliers
        $ateliers = $this->getDoctrine()->getRepository(Atelier::class)->findBy(['session'=>$id]);

        $json_session = [];
        //tableau associatif avec les informations de la session necessaire pour l'affichage dans calendar
        array_push($json_session, array(
            "id" => $session->getId(),
            "title" => $session->getNom(),
            "start" => $session->getDateDebut(),
            "end" => $session->getDateFin(),
            "allDay" => true,
            "color" => "#DA6552"
        ));

        $json_ateliers = [];
        foreach ($ateliers as $atelier){
            //recuperation des seances
            $seances = $atelier->getSeances();
            foreach ($seances as $seance) {
                //tableau associatif avec les informations de chaque seance necessaire pour l'affichage dans calendar
                array_push($json_ateliers, array(
                    "idSeance" => $seance->getId(),
                    "title" => $atelier->getNom(),
                    "start" => $seance->getHeureDebut(),
                    "end" => $seance->getHeureFin(),
                    "adresse" => $seance->getAdresse()
                ));
            }
        }

        return $this->json(array(
            $json_session,
            $json_ateliers
        ));
    }

    /**
     * @Method({"GET"})
     * permet de récupérer en json les valeurs nécessaires à la construction du formualire
     * de création d'une seance en jquery
     */
    public function getCreateSeanceModal($id){

        //recuperation de la session
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);
        //recuperation des ateliers
        $ateliers = $this->getDoctrine()->getRepository(Atelier::class)->findBy(['session'=>$id]);

        $sessionArray = [];
        array_push($sessionArray, array(
            "date_debut" => $session->getDateDebut()->format("Y-m-d"),
            "date_fin" => $session->getDateFin()->format("Y-m-d")
        ));

        $ateliersArray = [];
        foreach ($ateliers as $atelier){
            array_push($ateliersArray, array(
                "id" => $atelier->getId(),
                "nom" => $atelier->getNom()
            ));
        }

        return $this->json(array(
            $sessionArray,
            $ateliersArray
        ));
    }

    //permet de récupérer en json les propriétés d'une seance pour la création du formulaire dans calendar
    public function getModalSeance($id){
        $seance = $this->getDoctrine()->getRepository(Seance::class)->find($id);

        return $this->json(array(
            "id" => $seance->getId(),
            "title" => $seance->getAtelier()->getNom(),
            "description" => $seance->getAtelier()->getDescription(),
            "adresse" => $seance->getAdresse(),
            "start" => $seance->getHeureDebut()->format("H:i"),
            "end" => $seance->getHeureFin()->format("H:i")
        ));
    }

    //permet de récupérer en json les propriétés d'une session pour la création du formulaire dans calendar
    public function getModalSession($id){
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);

        return $this->json(array(
            "title" => $session->getNom(),
            "start" => $session->getDateDebut()->format("Y-m-d"),
            "end" => $session->getDateFin()->format("Y-m-d")
        ));
    }

    /**
     * @Method({"POST"})
     */
    public function updateCalendarSession(Request $request, $id){
        $em = $this->getDoctrine()->getManager();
        $post = $request->request->all();

        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);
        if($request->request->get('idSeance') !== null) {
            $seance = $this->getDoctrine()->getRepository(Seance::class)->find($post['idSeance']);
        }
        //recupération des dates des début et de fin de la session
        $dateDebut = $session->getDateDebut();
        $dateFin = $session->getDateFin();
        //calcule de l'interval
        $interval = $dateDebut->diff($dateFin);
        $dateInterval = new DateTime($post['start']);
        $dateInterval->add($interval);

        //si l'event modifier dans calendar correspond à la session
        if($request->request->get('title') === $session->getNom()){
            //aprés un drop seulement
            if ($request->request->get('start') !== "") {
                $session->setDateDebut(new DateTime($post['start']));
            }
            //aprés un resize seulement
            if ($request->request->get('end') !== "") {
                $session->setDateFin(new DateTime($post['end']));
            }
            //affecte l'interval à la date de fin aprés un drop
            else{
                $session->setDateFin(new DateTime($dateInterval->format('Y-m-d')));
            }
            $em->merge($session);
            $em->flush();
        }

        //recuperation de la date et l'heure de debut des seances envoyé par l'ajax
        $heureDebut = new DateTime($post['start']);
        //recuperation des heures de fin
        $heureFin = $seance->getHeureFin()->format("H:i");
        //formatage de la reception de l'ajax
        $jour = $heureDebut->format('Y-m-d');

        //si l'event modifier dans calendar correspond à une seance
        if ($request->request->get('title') === $seance->getAtelier()->getNom()) {
            //aprés un drop seulement
            if ($request->request->get('start') !== "") {
                $seance->setDate(new DateTime("$jour"));
                $seance->setHeureDebut(new DateTime($post['start']));
            }
            //aprés un resize seulement
            if ($request->request->get('end') !== "") {
                $seance->setHeureFin(new DateTime($post['end']));
            }
            //concatenation entre la nouvelle date de debut et l'heure de fin aprés un drop
            else{
                $seance->setHeureFin(new DateTime("$jour $heureFin"));
            }
            $em->merge($seance);
            $em->flush();
        }

        return $this->json(array("ok"));
    }

    //permet de supprimer une seance
    public function deleteSeance($id){
        $em = $this->getDoctrine()->getManager();
        $seance = $this->getDoctrine()->getRepository(Seance::class)->find($id);


        $em->remove($seance);
        $em->flush();

        return $this->json(array("ok"));
    }

    //permet de supprimer une session
    public function deleteSession($id){
        $em = $this->getDoctrine()->getManager();
        $session = $this->getDoctrine()->getRepository(Session::class)->find($id);


        $em->remove($session);
        $em->flush();

        return $this->json(array("ok"));
    }
}
