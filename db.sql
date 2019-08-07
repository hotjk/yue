-- MySQL dump 10.13  Distrib 5.7.25, for Win64 (x86_64)
--
-- Host: localhost    Database: yue
-- ------------------------------------------------------
-- Server version	5.7.25-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `booking_activities`
--

DROP TABLE IF EXISTS `booking_activities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `booking_activities` (
  `ActivityId` int(11) NOT NULL,
  `ResourceId` int(11) NOT NULL,
  `BookingId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `From` datetime NOT NULL,
  `To` datetime NOT NULL,
  `Minutes` int(11) NOT NULL,
  `Message` varchar(200) NOT NULL,
  `CreateBy` int(11) NOT NULL,
  `CreateAt` datetime NOT NULL,
  PRIMARY KEY (`ActivityId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `booking_activities`
--

LOCK TABLES `booking_activities` WRITE;
/*!40000 ALTER TABLE `booking_activities` DISABLE KEYS */;
INSERT INTO `booking_activities` VALUES (1,1,1,0,'2015-01-01 01:01:01','2015-01-01 02:01:01',60,'hello',9,'2019-08-07 14:40:07');
/*!40000 ALTER TABLE `booking_activities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bookings`
--

DROP TABLE IF EXISTS `bookings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bookings` (
  `ResourceId` int(11) NOT NULL,
  `BookingId` int(11) NOT NULL,
  `State` int(11) NOT NULL,
  `From` datetime NOT NULL,
  `To` datetime NOT NULL,
  `Minutes` int(11) NOT NULL,
  `CreateBy` int(11) NOT NULL,
  `CreateAt` datetime NOT NULL,
  `UpdateBy` int(11) NOT NULL,
  `UpdateAt` datetime NOT NULL,
  PRIMARY KEY (`BookingId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookings`
--

LOCK TABLES `bookings` WRITE;
/*!40000 ALTER TABLE `bookings` DISABLE KEYS */;
INSERT INTO `bookings` VALUES (1,1,1,'2015-01-01 01:01:01','2015-01-01 02:01:01',60,9,'2019-08-07 14:40:07',9,'2019-08-07 14:40:07');
/*!40000 ALTER TABLE `bookings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `members`
--

DROP TABLE IF EXISTS `members`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `members` (
  `MemberId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `GroupId` int(11) NOT NULL,
  `MemberGroup` int(11) NOT NULL,
  `CreateBy` int(11) NOT NULL,
  `CreateAt` datetime NOT NULL,
  PRIMARY KEY (`MemberId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `members`
--

LOCK TABLES `members` WRITE;
/*!40000 ALTER TABLE `members` DISABLE KEYS */;
/*!40000 ALTER TABLE `members` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sequence`
--

DROP TABLE IF EXISTS `sequence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sequence` (
  `Id` int(11) NOT NULL,
  `Value` int(11) NOT NULL DEFAULT '0',
  `Comment` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sequence`
--

LOCK TABLES `sequence` WRITE;
/*!40000 ALTER TABLE `sequence` DISABLE KEYS */;
INSERT INTO `sequence` VALUES (100,2,'Booking'),(101,2,'BookingAction'),(110,10,'User');
/*!40000 ALTER TABLE `sequence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_security`
--

DROP TABLE IF EXISTS `user_security`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_security` (
  `UserId` int(11) NOT NULL,
  `PasswordHash` varchar(200) NOT NULL,
  `ActivateToken` varchar(200) DEFAULT NULL,
  `ResetPasswordToken` varchar(200) DEFAULT NULL,
  `CreateBy` int(11) NOT NULL,
  `CreateAt` datetime NOT NULL,
  `UpdateBy` int(11) NOT NULL,
  `UpdateAt` datetime NOT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_security`
--

LOCK TABLES `user_security` WRITE;
/*!40000 ALTER TABLE `user_security` DISABLE KEYS */;
INSERT INTO `user_security` VALUES (9,'1000:nvOO3GjhE4HOSseipzJGw89eTYp13bXt:HAGXz35WiePPXm7r6wMTUR0haLxfIGr7',NULL,NULL,9,'2019-08-07 13:29:06',9,'2019-08-07 14:36:45');
/*!40000 ALTER TABLE `user_security` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_security_logs`
--

DROP TABLE IF EXISTS `user_security_logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_security_logs` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `PasswordHash` varchar(200) DEFAULT NULL,
  `Token` varchar(200) DEFAULT NULL,
  `CreateBy` int(11) NOT NULL,
  `CreateAt` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_security_logs`
--

LOCK TABLES `user_security_logs` WRITE;
/*!40000 ALTER TABLE `user_security_logs` DISABLE KEYS */;
INSERT INTO `user_security_logs` VALUES (1,9,0,'1000:nvOO3GjhE4HOSseipzJGw89eTYp13bXt:HAGXz35WiePPXm7r6wMTUR0haLxfIGr7',NULL,9,'2019-08-07 13:29:06'),(2,9,1,NULL,'c6a63c11-3f8f-4561-8d32-2802259307a3',9,'2019-08-07 14:26:37'),(3,9,1,NULL,'608f91fc-bbf1-4a80-8106-f2ced651da69',9,'2019-08-07 14:29:36'),(4,9,2,NULL,'608f91fc-bbf1-4a80-8106-f2ced651da69',9,'2019-08-07 14:36:45');
/*!40000 ALTER TABLE `user_security_logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `UserId` int(11) NOT NULL,
  `Email` varchar(200) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `State` int(11) NOT NULL,
  `CreateBy` int(11) NOT NULL,
  `CreateAt` datetime NOT NULL,
  `UpdateBy` int(11) NOT NULL,
  `UpdateAt` datetime NOT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (9,'zhongwx@gmail.com','weixiao',2,9,'2019-08-07 13:29:06',9,'2019-08-07 13:29:06');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-08-07 15:42:52
