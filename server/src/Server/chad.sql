CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `FriendlyName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Role` int NOT NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
);

CREATE TABLE `Configs` (
    `Type` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Configs` PRIMARY KEY (`Type`)
);

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Classes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `DirectorId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Classes` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Classes_AspNetUsers_DirectorId` FOREIGN KEY (`DirectorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Courses` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `DirectorId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Courses` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Courses_AspNetUsers_DirectorId` FOREIGN KEY (`DirectorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Messages` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Content` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `Time` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `SenderId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ReceiverId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Messages` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Messages_AspNetUsers_ReceiverId` FOREIGN KEY (`ReceiverId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Messages_AspNetUsers_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Resources` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `Content` longblob NOT NULL,
    `UploadTime` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `Expired` datetime(6) NOT NULL,
    `UploaderId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ContentType` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Resources` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Resources_AspNetUsers_UploaderId` FOREIGN KEY (`UploaderId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RelStudentClasses` (
    `StudentId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClassId` bigint NOT NULL,
    CONSTRAINT `PK_RelStudentClasses` PRIMARY KEY (`StudentId`, `ClassId`),
    CONSTRAINT `FK_RelStudentClasses_AspNetUsers_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RelStudentClasses_Classes_ClassId` FOREIGN KEY (`ClassId`) REFERENCES `Classes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Lessons` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `Index` smallint unsigned NOT NULL,
    `Description` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `CourseId` bigint NOT NULL,
    CONSTRAINT `PK_Lessons` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Lessons_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RelCourseClasses` (
    `CourseId` bigint NOT NULL,
    `ClassId` bigint NOT NULL,
    CONSTRAINT `PK_RelCourseClasses` PRIMARY KEY (`ClassId`, `CourseId`),
    CONSTRAINT `FK_RelCourseClasses_Classes_ClassId` FOREIGN KEY (`ClassId`) REFERENCES `Classes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RelCourseClasses_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RelResourceLessons` (
    `LessonId` bigint NOT NULL,
    `ResourceId` bigint NOT NULL,
    `Index` smallint unsigned NOT NULL,
    CONSTRAINT `PK_RelResourceLessons` PRIMARY KEY (`ResourceId`, `LessonId`),
    CONSTRAINT `FK_RelResourceLessons_Lessons_LessonId` FOREIGN KEY (`LessonId`) REFERENCES `Lessons` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RelResourceLessons_Resources_ResourceId` FOREIGN KEY (`ResourceId`) REFERENCES `Resources` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE INDEX `IX_Classes_DirectorId` ON `Classes` (`DirectorId`);

CREATE INDEX `IX_Courses_DirectorId` ON `Courses` (`DirectorId`);

CREATE INDEX `IX_Lessons_CourseId` ON `Lessons` (`CourseId`);

CREATE INDEX `IX_Messages_ReceiverId` ON `Messages` (`ReceiverId`);

CREATE INDEX `IX_Messages_SenderId_ReceiverId` ON `Messages` (`SenderId`, `ReceiverId`);

CREATE INDEX `IX_RelCourseClasses_ClassId_CourseId` ON `RelCourseClasses` (`ClassId`, `CourseId`);

CREATE INDEX `IX_RelCourseClasses_CourseId` ON `RelCourseClasses` (`CourseId`);

CREATE INDEX `IX_RelResourceLessons_LessonId_ResourceId` ON `RelResourceLessons` (`LessonId`, `ResourceId`);

CREATE INDEX `IX_RelStudentClasses_ClassId_StudentId` ON `RelStudentClasses` (`ClassId`, `StudentId`);

CREATE INDEX `IX_Resources_UploaderId` ON `Resources` (`UploaderId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210416083252_Init', '5.0.3');

COMMIT;

