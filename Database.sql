-- Database.sql

CREATE DATABASE UnityGame;

USE UnityGame;

-- Table for Users
CREATE TABLE Users (
    UserID INT AUTO_INCREMENT,
    UserName VARCHAR(255) NOT NULL,
    AvatarID INT,
    PRIMARY KEY(UserID),
    UNIQUE(UserName)
);

-- Table for Avatars
CREATE TABLE Avatars (
    AvatarID INT AUTO_INCREMENT,
    AvatarName VARCHAR(255),
    Avatar3DFile VARCHAR(255),
    AvatarSkin VARCHAR(255),
    AvatarWeight FLOAT,
    PRIMARY KEY(AvatarID)
);

-- Table for Fighting Styles
CREATE TABLE FightingStyles (
    StyleID INT AUTO_INCREMENT,
    StyleName VARCHAR(255),
    PRIMARY KEY(StyleID)
);

-- Table for Rounds
CREATE TABLE Rounds (
    RoundID INT AUTO_INCREMENT,
    StartTime TIMESTAMP,
    EndTime TIMESTAMP,
    WinnerID INT,
    PRIMARY KEY(RoundID)
);

-- Table for UserRound to keep track of user's participation in rounds
CREATE TABLE UserRound (
    UserID INT,
    RoundID INT,
    StyleID INT,
    AvatarID INT,
    Health FLOAT,
    PositionX FLOAT,
    PositionY FLOAT,
    PositionZ FLOAT,
    FOREIGN KEY(UserID) REFERENCES Users(UserID),
    FOREIGN KEY(RoundID) REFERENCES Rounds(RoundID),
    FOREIGN KEY(StyleID) REFERENCES FightingStyles(StyleID),
    FOREIGN KEY(AvatarID) REFERENCES Avatars(AvatarID)
);

-- Table for Maps
CREATE TABLE Maps (
    MapID INT AUTO_INCREMENT,
    MapFile VARCHAR(255),
    PRIMARY KEY(MapID)
);

-- Table for MapRound to keep track of which map was used in each round
CREATE TABLE MapRound (
    MapID INT,
    RoundID INT,
    FOREIGN KEY(MapID) REFERENCES Maps(MapID),
    FOREIGN KEY(RoundID) REFERENCES Rounds(RoundID)
);
