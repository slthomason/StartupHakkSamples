USE [master]
GO

/****** Object:  Database [Store]    Script Date: 6/4/2024 10:46:37 PM ******/
CREATE DATABASE [Store]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Store', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Store.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Store_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Store_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Store].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Store] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Store] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Store] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Store] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Store] SET ARITHABORT OFF 
GO

ALTER DATABASE [Store] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Store] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Store] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Store] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Store] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Store] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Store] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Store] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Store] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Store] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Store] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Store] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Store] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Store] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Store] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Store] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Store] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Store] SET RECOVERY FULL 
GO

ALTER DATABASE [Store] SET  MULTI_USER 
GO

ALTER DATABASE [Store] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Store] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Store] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Store] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Store] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Store] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [Store] SET QUERY_STORE = OFF
GO

ALTER DATABASE [Store] SET  READ_WRITE 
GO


