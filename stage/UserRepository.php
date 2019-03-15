<?php

namespace App\Repository;

use App\Entity\User;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Symfony\Bridge\Doctrine\RegistryInterface;

/**
 * @method User|null find($id, $lockMode = null, $lockVersion = null)
 * @method User|null findOneBy(array $criteria, array $orderBy = null)
 * @method User[]    findAll()
 * @method User[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class UserRepository extends ServiceEntityRepository
{
    public function __construct(RegistryInterface $registry)
    {
        parent::__construct($registry, User::class);
    }

    public function findByCreated($value)
    {
        $qb = $this->createQueryBuilder('u');

        $qb->join('u.cree_par', 'a')
            ->where('a = :admin')
            ->setParameter('admin', $value)
            ->getQuery()
            ->getResult();
        return $qb;
    }

//    public function findByCreated($value){
//        return $this->createQueryBuilder('u')
//         ->select('*')
//         ->from('User', 'u')
//         ->where('u.cree_par',$value);
//    }
//    /**
//     * @return User[] Returns an array of User objects
//     */
        /*
        public function findByExampleField($value)
        {
            return $this->createQueryBuilder('u')
                ->andWhere('u.exampleField = :val')
                ->setParameter('val', $value)
                ->orderBy('u.id', 'ASC')
                ->setMaxResults(10)
                ->getQuery()
                ->getResult()
            ;
        }
        */


    /*
    public function findOneBySomeField($value): ?User
    {
        return $this->createQueryBuilder('u')
            ->andWhere('u.exampleField = :val')
            ->setParameter('val', $value)
            ->getQuery()
            ->getOneOrNullResult()
        ;
    }
    */
}
