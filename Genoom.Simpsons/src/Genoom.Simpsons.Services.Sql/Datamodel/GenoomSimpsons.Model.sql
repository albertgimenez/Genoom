CREATE TABLE Person
(
	Id uniqueidentifier DEFAULT NEWID(),
	Name varchar(100) NOT NULL,
	Lastname varchar(100),
	Birthdate datetime,
	Sex tinyint NOT NULL,
	PhotoFileName varchar(255),

	PRIMARY KEY (Id)
);

CREATE TABLE PersonFamily
(
	PersonId uniqueidentifier,
	RelatedPersonId uniqueidentifier,
	RelationShip tinyint NOT NULL,

	PRIMARY KEY (PersonId, RelatedPersonId),
	FOREIGN KEY (PersonId) REFERENCES Person(Id),
	FOREIGN KEY (RelatedPersonId) REFERENCES Person(Id)
);

GO

CREATE VIEW PersonRelationship AS
(
	SELECT F.PersonId, P1.Name, P1.Lastname, F.RelatedPersonId, P2.Name RelatedName, P2.Lastname RelatedLastName, F.RelationShip
	FROM PersonFamily F
	LEFT JOIN Person P1 ON F.PersonId=P1.Id
	LEFT JOIN Person P2 ON F.RelatedPersonId=P2.Id
);