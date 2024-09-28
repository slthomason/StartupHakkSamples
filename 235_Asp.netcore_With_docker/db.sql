create database restaurant;
use restaurant;

CREATE TABLE [dbo].[categories](
	[catId] [int] IDENTITY(1,1) NOT NULL,
	[catName] [varchar](50) NOT NULL
);

CREATE TABLE [dbo].[products](
	[productId] [int] IDENTITY(1,1) NOT NULL,
	[productName] [varchar](50) NULL,
	[price] [decimal](4, 2) NULL,
	[catId] [int] NULL
);