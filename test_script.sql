CREATE TABLE `test`.`contacto` (
  `idcontacto` INT NOT NULL,
  `descripcion` VARCHAR(255) NULL,
  `pdf` VARCHAR(255) NULL,
  PRIMARY KEY (`idcontacto`));

CREATE TABLE `test`.`informacio` (
  `idinformacio` INT NOT NULL,
  `descripcion` VARCHAR(255) NULL,
  `data` DATETIME NULL,
  PRIMARY KEY (`idinformacio`));
