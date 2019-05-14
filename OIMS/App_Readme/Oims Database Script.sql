-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.1.53-community - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for table inventory.allocated
CREATE TABLE IF NOT EXISTS `allocated` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `OrderId` int(11) NOT NULL,
  `ItemId` int(11) NOT NULL,
  `MarkQty` float NOT NULL COMMENT 'The quantity that is recommended by the Supervisor. And same amount will be reduced from item stock.',
  PRIMARY KEY (`Id`),
  KEY `FK_Alloc_OrderId` (`OrderId`),
  KEY `FK_Alloc_ItemId` (`ItemId`),
  KEY `FK_Alloc_UserId` (`UserId`),
  CONSTRAINT `FK_Alloc_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `items` (`I_Id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `FK_Alloc_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `requests` (`O_Id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `FK_Alloc_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`U_Id`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Used to keep records of items recommended to users.';

-- Data exporting was unselected.


-- Dumping structure for table inventory.cartstuff
CREATE TABLE IF NOT EXISTS `cartstuff` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `U_Id` int(11) NOT NULL,
  `P_Id` int(11) DEFAULT NULL,
  `I_Id` int(11) DEFAULT NULL,
  `Quantity` float NOT NULL DEFAULT '0',
  `CreatedOnUtc` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_UserId` (`U_Id`),
  KEY `FK_ProductId` (`P_Id`),
  KEY `FK_ItemId` (`I_Id`),
  CONSTRAINT `FK_ItemId` FOREIGN KEY (`I_Id`) REFERENCES `items` (`I_Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ProductId` FOREIGN KEY (`P_Id`) REFERENCES `shelters` (`P_Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_UserId` FOREIGN KEY (`U_Id`) REFERENCES `users` (`U_Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Used to keep track of items in cart for each users.';


-- Dumping structure for table inventory.companies
CREATE TABLE IF NOT EXISTS `companies` (
  `CompanyName` varchar(100) NOT NULL,
  PRIMARY KEY (`CompanyName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Used to keep record of companies.';

-- Dumping data for table inventory.companies: ~5 rows (approximately)
DELETE FROM `companies`;
/*!40000 ALTER TABLE `companies` DISABLE KEYS */;
INSERT INTO `companies` (`CompanyName`) VALUES
	('491 Fd'),
	('496 Fd'),
	('499 Fd'),
	('620 Fd Pk'),
	('RHQ');

-- Data exporting was unselected.


-- Dumping structure for table inventory.items
CREATE TABLE IF NOT EXISTS `items` (
  `I_Id` int(10) NOT NULL AUTO_INCREMENT,
  `I_Name` varchar(200) NOT NULL,
  `I_Quantity` float NOT NULL,
  `Size` varchar(50) NOT NULL,
  `Marking` varchar(50) NOT NULL,
  `Vendor` varchar(100) DEFAULT NULL,
  `UpdatedOn` datetime DEFAULT NULL,
  PRIMARY KEY (`I_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Used to keep list of items available in inventory. ';

-- Data exporting was unselected.


-- Dumping structure for table inventory.itemslog
CREATE TABLE IF NOT EXISTS `itemslog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` int(11) NOT NULL,
  `ItemQuantity` float NOT NULL,
  `VendorName` varchar(100) NOT NULL,
  `EntryDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FKLog_ItemId` (`ItemId`),
  KEY `FKLog_VName` (`VendorName`),
  CONSTRAINT `FKLog_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `items` (`I_Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FKLog_VName` FOREIGN KEY (`VendorName`) REFERENCES `vendors` (`Name`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Keep log of incoming items from vendors.';

-- Data exporting was unselected.


-- Dumping structure for table inventory.itemsrequest
CREATE TABLE IF NOT EXISTS `itemsrequest` (
  `OI_Id` int(10) NOT NULL AUTO_INCREMENT,
  `O_Id` int(10) NOT NULL,
  `I_Id` int(10) NOT NULL,
  `OI_Quantity` float NOT NULL COMMENT 'The quantity requested by requestor',
  `OI_ItemRecom` float DEFAULT NULL,
  `OI_ItemAlloc` float DEFAULT NULL,
  `OI_Pending` float DEFAULT NULL,
  `OI_CreatedBy` int(10) NOT NULL,
  `OI_CreatedDate` datetime NOT NULL,
  `OI_UpdatedBy` int(11) DEFAULT NULL,
  `OI_UpdatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`OI_Id`),
  KEY `FK_OrderId` (`O_Id`),
  KEY `FK_Order_ItemId` (`I_Id`),
  CONSTRAINT `FK_OrderId` FOREIGN KEY (`O_Id`) REFERENCES `requests` (`O_Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_Order_ItemId` FOREIGN KEY (`I_Id`) REFERENCES `items` (`I_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Keep items requested per user wise.';

-- Dumping structure for table inventory.requests
CREATE TABLE IF NOT EXISTS `requests` (
  `O_Id` int(10) NOT NULL AUTO_INCREMENT,
  `O_CreatedBy` int(11) NOT NULL,
  `O_CreatedDate` datetime NOT NULL,
  `O_UpdatedBy` int(11) DEFAULT NULL,
  `O_UpdatedDate` datetime DEFAULT NULL,
  `O_Job` varchar(100) DEFAULT NULL,
  `O_Status` int(11) NOT NULL,
  `O_Company` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`O_Id`),
  KEY `FK_OrderStatus` (`O_Status`),
  KEY `FK_User` (`O_CreatedBy`),
  KEY `FK_Company` (`O_Company`),
  CONSTRAINT `FK_Company` FOREIGN KEY (`O_Company`) REFERENCES `companies` (`CompanyName`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_OrderStatus` FOREIGN KEY (`O_Status`) REFERENCES `requeststatus` (`S_Id`) ON UPDATE CASCADE,
  CONSTRAINT `FK_User` FOREIGN KEY (`O_CreatedBy`) REFERENCES `users` (`U_Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Common table for managing requests.';

-- Dumping structure for table inventory.requeststatus
CREATE TABLE IF NOT EXISTS `requeststatus` (
  `S_Id` int(11) NOT NULL,
  `S_Name` tinytext NOT NULL,
  `ForRole` int(11) NOT NULL,
  PRIMARY KEY (`S_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Different status. ForRole - 0 stands for common.';

-- Dumping data for table inventory.requeststatus: ~6 rows (approximately)
DELETE FROM `requeststatus`;
/*!40000 ALTER TABLE `requeststatus` DISABLE KEYS */;
INSERT INTO `requeststatus` (`S_Id`, `S_Name`, `ForRole`) VALUES
	(1, 'In Process', 2),
	(2, 'Manager approval pending', 2),
	(3, 'Approved', 0),
	(4, 'Rejected', 0),
	(5, 'Partial approved', 2),
	(6, 'Closed', 2);

-- Dumping structure for table inventory.role
CREATE TABLE IF NOT EXISTS `role` (
  `R_Id` int(10) NOT NULL AUTO_INCREMENT,
  `R_Name` varchar(200) NOT NULL,
  PRIMARY KEY (`R_Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='Roles for users.';

-- Dumping data for table inventory.role: ~3 rows (approximately)
DELETE FROM `role`;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` (`R_Id`, `R_Name`) VALUES
	(1, 'Manager'),
	(2, 'Supervisor'),
	(3, 'Requestor');
-- Data exporting was unselected.


-- Dumping structure for table inventory.shelterdescription
CREATE TABLE IF NOT EXISTS `shelterdescription` (
  `PD_Id` int(10) NOT NULL AUTO_INCREMENT,
  `P_Id` int(10) NOT NULL,
  `I_Id` int(10) NOT NULL,
  `I_Qty` int(10) NOT NULL,
  PRIMARY KEY (`PD_Id`),
  KEY `FK__products` (`P_Id`),
  KEY `FK_items` (`I_Id`),
  CONSTRAINT `FK_items` FOREIGN KEY (`I_Id`) REFERENCES `items` (`I_Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK__products` FOREIGN KEY (`P_Id`) REFERENCES `shelters` (`P_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='List of items that make particular shelter.';

-- Data exporting was unselected.


-- Dumping structure for table inventory.shelters
CREATE TABLE IF NOT EXISTS `shelters` (
  `P_Id` int(10) NOT NULL AUTO_INCREMENT,
  `P_Name` varchar(200) NOT NULL,
  `P_Dummy` int(10) DEFAULT NULL,
  PRIMARY KEY (`P_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Keep record of shelters';

-- Data exporting was unselected.


-- Dumping structure for table inventory.sheltersrequest
CREATE TABLE IF NOT EXISTS `sheltersrequest` (
  `OP_Id` int(10) NOT NULL AUTO_INCREMENT,
  `O_Id` int(10) NOT NULL,
  `P_Id` int(10) NOT NULL,
  `OP_Quantity` float NOT NULL,
  `OP_ProdRecom` int(11) DEFAULT NULL,
  `OP_Pending` int(11) DEFAULT NULL,
  `OP_ProdAlloc` int(11) DEFAULT NULL,
  `OP_CreatedBy` int(10) NOT NULL,
  `OP_CreatedDate` datetime NOT NULL,
  `OP_UpdatedBy` int(10) DEFAULT NULL,
  `OP_UpdatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`OP_Id`),
  KEY `FK__orders` (`O_Id`),
  KEY `FK_Order_ProductId` (`P_Id`),
  CONSTRAINT `FK_Order_ProductId` FOREIGN KEY (`P_Id`) REFERENCES `shelters` (`P_Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK__orders` FOREIGN KEY (`O_Id`) REFERENCES `requests` (`O_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Keep items requested per user wise.';

-- Data exporting was unselected.


-- Dumping structure for table inventory.users
CREATE TABLE IF NOT EXISTS `users` (
  `U_Id` int(10) NOT NULL AUTO_INCREMENT,
  `R_Id` int(10) NOT NULL,
  `U_FirstName` varchar(200) NOT NULL,
  `U_LastName` varchar(200) DEFAULT NULL,
  `U_Email` varchar(200) NOT NULL,
  `U_Password` varchar(200) NOT NULL,
  PRIMARY KEY (`U_Id`),
  KEY `FK_Role` (`R_Id`),
  CONSTRAINT `FK_Role` FOREIGN KEY (`R_Id`) REFERENCES `role` (`R_Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Users who will be using OIMS.';

INSERT INTO `users` (`U_Id`, `R_Id`, `U_FirstName`, `U_LastName`, `U_Email`, `U_Password`) VALUES
	(1, 2, 'Administrator', NULL, 'admin@gmail.com', '123456');
-- Data exporting was unselected.


-- Dumping structure for table inventory.vendors
CREATE TABLE IF NOT EXISTS `vendors` (
  `Name` varchar(100) NOT NULL,
  `Address` varchar(100) DEFAULT NULL,
  `Phone` varchar(10) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='List of vendors who will be sending items.';

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
