CREATE SCHEMA IF NOT EXISTS `3dsessiondb` DEFAULT CHARACTER SET utf8;

CREATE TABLE IF NOT EXISTS `3dsessiondb`.`Instance` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `externalId` VARCHAR(100) NOT NULL,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(300) NULL,
  `creationTimeStamp` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC),
  UNIQUE INDEX `externalId_UNIQUE` (`externalId` ASC),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `3dsessiondb`.`MessageLog` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `externalID` VARCHAR(50) NOT NULL,
  `timestamp` DATETIME NOT NULL,
  `dataType` VARCHAR(50) NOT NULL,
  `data` LONGTEXT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `3dsessiondb`.`Setup` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(500) NULL,
  `creationTimestamp` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `3dsessiondb`.`Location` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `setupId` INT NOT NULL,
  `instanceId` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `description` VARCHAR(500) NULL,
  `creationTimestamp` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `setup_location_fk_idx` (`setupId` ASC),
  INDEX `instance_location_fk_idx` (`instanceId` ASC),
  CONSTRAINT `setup_location_fk`
    FOREIGN KEY (`setupId`)
    REFERENCES `3dsessiondb`.`Setup` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `instance_location_fk`
    FOREIGN KEY (`instanceId`)
    REFERENCES `3dsessiondb`.`Instance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `3dsessiondb`.`Session` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `setupId` INT NOT NULL,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(500) NULL,
  `startTime` DATETIME NULL,
  `endTime` DATETIME NULL,
  `isActive` TINYINT(1) NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC),
  INDEX `setupID_fk_idx` (`setupId` ASC),
  CONSTRAINT `setupID_fk`
    FOREIGN KEY (`setupId`)
    REFERENCES `3dsessiondb`.`Setup` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `3dsessiondb`.`SessionData` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `sessionId` INT NOT NULL,
  `instanceId` INT NOT NULL,
  `dataType` VARCHAR(45) NOT NULL,
  `data` LONGTEXT NULL,
  `dataBlob` BLOB NULL,
  `timeStamp` DATETIME NOT NULL,
  `processedData` BLOB NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC),
  INDEX `session_sessiondata_fk_idx` (`sessionId` ASC),
  INDEX `instance_sessiondata_fk_idx` (`instanceId` ASC),
  CONSTRAINT `session_sessiondata_fk`
    FOREIGN KEY (`sessionId`)
    REFERENCES `3dsessiondb`.`Session` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `instance_sessiondata_fk`
    FOREIGN KEY (`instanceId`)
    REFERENCES `3dsessiondb`.`Instance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;