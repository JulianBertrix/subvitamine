<?php

namespace App\Controller;

use App\Entity\Atelier;
use App\Entity\Idee;
use App\Entity\Message;
use App\Entity\Seance;
use App\Entity\Session;
use App\Entity\User;
use DateTime;
use Sensio\Bundle\FrameworkExtraBundle\Configuration\Method;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;

class AtelierController extends AbstractController
{
    //affichage de la vue
    public function index($id)
    {
        //récupération de la seance
        $seance = $this->getDoctrine()->getRepository(Seance::class)->find($id);
        //récupération de l'atelier
        $atelier = $seance->getAtelier();
        //récuperation des idées liées à un atelier (au cas ou le même atelier aurait lieu plusieurs fois)
        $idees = $this->getDoctrine()->getRepository(Idee::class)->findBy(['atelier'=>$atelier->getId()]);

        //récupération de l'interval pour le timer
        $debut = $seance->getHeureDebut();
        $fin = $seance->getHeureFin();
        $interval = $debut->diff($fin);
        $dateInterval = new DateTime();

        return $this->render('atelier/index.html.twig', [
            'controller_name' => 'AtelierController',
            'seance' => $seance,
            'atelier' => $atelier,
            'idees' => $idees,
            'interval' => $dateInterval->add($interval)
        ]);
    }

    //Pour la reconstruction de la liste des idées en jquery
    public function getIdeaList($id){
        //récuperation des idées liées à l'atelier
        $idees = $this->getDoctrine()->getRepository(Idee::class)->findBy(['atelier' => $id]);

        //initialisation du tableau à retourner en json
        $ideesArray = [];

        foreach ($idees as $idee){
            //inserer les idées dans le tableau
            array_push($ideesArray, array(
                "intitule" => $idee->getIntitule()
            ));
        }

        return $this->json($ideesArray);
    }

    //ajouter une nouvelle idée
    public function addIdea(Request $request, $id){
        //recupération des superglobals
        $post = $request->request->all();

        $em = $this->getDoctrine()->getManager();

        //creation d'une nouvelle idée
        $idee = new Idee();
        $idee->setIntitule($post['nom']);
        $idee->setAtelier($em->find(Atelier::class, $id));
        $em->persist($idee);
        $em->flush();

        return $this->json("success");
    }

    public function getFacilitateurs(){
        //recupétation de tous les facilitateurs
        $allFacilitateurs = $this->getDoctrine()->getRepository(User::class)->findBy(['roles'=>'ROLE_FACILITATEUR']);
        //déclaration d'un tableau
        $facilitateursArray = [];

        foreach ($allFacilitateurs as $facilitateur){
            //inserer dans le tableau un facilitateur connecter
            if($facilitateur->getId() === $this->getUser()->getId()){
                array_push($facilitateursArray, $facilitateur);
            }
        }

        return $this->json($facilitateursArray);
    }

    //affichage des message dans le chat
    public function afficherMessage($id){
        //récupération de la seance
        $seance = $this->getDoctrine()->getRepository(Seance::class)->find($id);
        //récupération des messages envoyés
        $messages = $this->getDoctrine()->getRepository(Message::class)->findBy(['seance'=>$seance->getId()]);

        //initialisation d'un tableau de message à renvoyer en json
        $messageArray = [];
        foreach ($messages as $message){
            //inserer les message dans le tableau
            array_push($messageArray, array(
                'id'=>$message->getId(),
                'idFacilitateur' => $message->getFacilitateur()->getId(),
                'text'=>$message->getText(),
                'nom'=>$message->getFacilitateur()->getNom(),
                'prenom'=>$message->getFacilitateur()->getPrenom()
            ));
        }

        return $this->json($messageArray);
    }

    //ajout d'un nouveau message dans le chat
    public function saveMessage(Request $request, $id){
        //récupération de la seance
        $seance = $this->getDoctrine()->getRepository(Seance::class)->find($id);
        //recupération des superglobals
        $post = $request->request->all();

        $em = $this->getDoctrine()->getManager();

        //creation du message
        $message = new Message();
        $message->setText($post['msg']);
        $message->setFacilitateur($em->find(User::class, $this->getUser()->getId()));
        $message->setSeance($em->find(Seance::class, $seance->getId()));
        $em->persist($message);
        $em->flush();

        //retour en json des propriétés du message nécéssaire pour le chat
        return $this->json(array(
            'id'=>$message->getId(),
            'text'=>$message->getText(),
            'nom'=>$message->getFacilitateur()->getNom(),
            'prenom'=>$message->getFacilitateur()->getPrenom()
        ));
    }

    //fonction qui permet de supprimer les facilitateur d'un atelier qui se trouve dans le carousel
    /**
     * @Method({"POST"})
     */
    public function removeFacilitateur($idA, $idF){
        //recuperation de l'atelier
        $atelier = $this->getDoctrine()->getRepository(Atelier::class)->find($idA);
        //recuperation du facilitateur
        $facilitateur = $this->getDoctrine()->getRepository(User::class)->find($idF);

        //retirer le facilitateur de l'atelier
        $atelier->removeFacilitateur($facilitateur);
        $this->getDoctrine()->getManager()->flush();

        return $this->json(array("success"));
    }
}
