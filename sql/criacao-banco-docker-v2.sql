USE [master]
GO
/****** Object:  Database [NerdStoreEnterpriseDB]    Script Date: 06/08/2023 18:11:19 ******/
CREATE DATABASE [NerdStoreEnterpriseDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NerdStoreEnterpriseDB', FILENAME = N'/var/opt/mssql/data/NerdStoreEnterpriseDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NerdStoreEnterpriseDB_log', FILENAME = N'/var/opt/mssql/data/NerdStoreEnterpriseDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NerdStoreEnterpriseDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET RECOVERY FULL 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET  MULTI_USER 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'NerdStoreEnterpriseDB', N'ON'
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET QUERY_STORE = OFF
GO
USE [NerdStoreEnterpriseDB]
GO
USE [NerdStoreEnterpriseDB]
GO
/****** Object:  Sequence [dbo].[MinhaSequencia]    Script Date: 06/08/2023 18:11:19 ******/
CREATE SEQUENCE [dbo].[MinhaSequencia] 
 AS [int]
 START WITH 1000
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO
USE [NerdStoreEnterpriseDB]
GO
/****** Object:  Sequence [dbo].[NumeroPedidos]    Script Date: 06/08/2023 18:11:19 ******/
CREATE SEQUENCE [dbo].[NumeroPedidos] 
 AS [int]
 START WITH 1000
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarrinhoCliente]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarrinhoCliente](
	[Id] [uniqueidentifier] NOT NULL,
	[ClienteId] [uniqueidentifier] NOT NULL,
	[ValorTotal] [decimal](18, 2) NOT NULL,
	[Desconto] [decimal](18, 2) NOT NULL,
	[Percentual] [decimal](18, 2) NULL,
	[TipoDesconto] [int] NOT NULL,
	[ValorDesconto] [decimal](18, 2) NULL,
	[VoucherCodigo] [varchar](250) NOT NULL,
	[VoucherUtilizado] [bit] NOT NULL,
 CONSTRAINT [PK_CarrinhoCliente] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarrinhoItens]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarrinhoItens](
	[Id] [uniqueidentifier] NOT NULL,
	[ProdutoId] [uniqueidentifier] NOT NULL,
	[Nome] [nvarchar](250) NOT NULL,
	[Quantidade] [int] NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[Imagem] [nvarchar](250) NOT NULL,
	[CarrinhoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CarrinhoItens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[Id] [uniqueidentifier] NOT NULL,
	[Nome] [varchar](250) NOT NULL,
	[Email] [varchar](250) NOT NULL,
	[Cpf] [varchar](11) NOT NULL,
	[Excluido] [bit] NOT NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enderecos]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enderecos](
	[Id] [uniqueidentifier] NOT NULL,
	[Logradouro] [varchar](250) NOT NULL,
	[Numero] [varchar](250) NOT NULL,
	[Complemento] [varchar](250) NOT NULL,
	[Bairro] [varchar](250) NOT NULL,
	[Cep] [varchar](250) NOT NULL,
	[Cidade] [varchar](250) NOT NULL,
	[Estado] [varchar](250) NOT NULL,
	[ClienteId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Enderecos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagamentos]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagamentos](
	[Id] [uniqueidentifier] NOT NULL,
	[PedidoId] [uniqueidentifier] NOT NULL,
	[TipoPagamento] [int] NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Pagamentos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PedidoItems]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PedidoItems](
	[Id] [uniqueidentifier] NOT NULL,
	[PedidoId] [uniqueidentifier] NOT NULL,
	[ProdutoId] [uniqueidentifier] NOT NULL,
	[ProdutoNome] [varchar](250) NOT NULL,
	[Quantidade] [int] NOT NULL,
	[ValorUnitario] [decimal](18, 2) NOT NULL,
	[ProdutoImagem] [nvarchar](250) NULL,
 CONSTRAINT [PK_PedidoItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedidos]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[Id] [uniqueidentifier] NOT NULL,
	[Codigo] [int] NOT NULL,
	[ClienteId] [uniqueidentifier] NOT NULL,
	[VoucherId] [uniqueidentifier] NULL,
	[VoucherUtilizado] [bit] NOT NULL,
	[Desconto] [decimal](18, 2) NOT NULL,
	[ValorTotal] [decimal](18, 2) NOT NULL,
	[DataCadastro] [datetime2](7) NOT NULL,
	[PedidoStatus] [int] NOT NULL,
	[Logradouro] [nvarchar](250) NULL,
	[Numero] [nvarchar](250) NULL,
	[Complemento] [nvarchar](250) NULL,
	[Bairro] [nvarchar](250) NULL,
	[Cep] [nvarchar](250) NULL,
	[Cidade] [nvarchar](250) NULL,
	[Estado] [nvarchar](250) NULL,
 CONSTRAINT [PK_Pedidos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Produtos]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produtos](
	[Id] [uniqueidentifier] NOT NULL,
	[Nome] [varchar](250) NOT NULL,
	[Descricao] [varchar](250) NOT NULL,
	[Ativo] [bit] NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[DataCadastro] [datetime2](7) NOT NULL,
	[Imagem] [varchar](250) NOT NULL,
	[QuantidadeEstoque] [int] NOT NULL,
 CONSTRAINT [PK_Produtos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](max) NULL,
	[Token] [uniqueidentifier] NOT NULL,
	[ExpirationDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SecurityKeys]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityKeys](
	[Id] [uniqueidentifier] NOT NULL,
	[KeyId] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[Parameters] [nvarchar](max) NULL,
	[IsRevoked] [bit] NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[ExpiredAt] [datetime2](7) NULL,
	[RevokedReason] [nvarchar](max) NULL,
 CONSTRAINT [PK_SecurityKeys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transacoes]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transacoes](
	[Id] [uniqueidentifier] NOT NULL,
	[CodigoAutorizacao] [nvarchar](250) NULL,
	[BandeiraCartao] [nvarchar](250) NULL,
	[DataTransacao] [datetime2](7) NULL,
	[ValorTotal] [decimal](18, 2) NOT NULL,
	[CustoTransacao] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[TID] [nvarchar](250) NULL,
	[NSU] [nvarchar](250) NULL,
	[PagamentoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Transacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vouchers]    Script Date: 06/08/2023 18:11:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vouchers](
	[Id] [uniqueidentifier] NOT NULL,
	[Codigo] [varchar](250) NOT NULL,
	[Percentual] [decimal](18, 2) NULL,
	[ValorDesconto] [decimal](18, 2) NULL,
	[Quantidade] [int] NOT NULL,
	[TipoDesconto] [int] NOT NULL,
	[DataCriacao] [datetime2](7) NOT NULL,
	[DataUtilizacao] [datetime2](7) NULL,
	[DataValidade] [datetime2](7) NOT NULL,
	[Ativo] [bit] NOT NULL,
	[Utilizado] [bit] NOT NULL,
 CONSTRAINT [PK_Vouchers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a11c-08eb80a9060b', N'Camiseta 4 Head', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'4head.webp', 4)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a12c-08eb80a9060b', N'Camiseta 4 Head Branca', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Branca 4head.webp', 5)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a13c-08eb80a9060b', N'Camiseta Tiltado', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'tiltado.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a14c-08eb80a9060b', N'Camiseta Tiltado Branca', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Branco Tiltado.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a15c-08eb80a9060b', N'Camiseta Heisenberg', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Heisenberg.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a16c-08eb80a9060b', N'Camiseta Kappa', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Kappa.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a17c-08eb80a9060b', N'Camiseta MacGyver', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'MacGyver.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a18c-08eb80a9060b', N'Camiseta Maestria', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Maestria.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a19c-08eb80a9060b', N'Camiseta Code Life Preta', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(60.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'camiseta2.jpg', 8)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a29c-08eb80a9060b', N'Camiseta My Yoda', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'My Yoda.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a39c-08eb80a9060b', N'Camiseta Pato', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Pato.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a41c-08eb80a9060b', N'Camiseta Xavier School', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Xaviers School.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a42c-08eb80a9060b', N'Camiseta Yoda', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Yoda.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a49c-08eb80a9060b', N'Camiseta Quack', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Quack.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a59c-08eb80a9060b', N'Camiseta Rick And Morty 2', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Rick And Morty Captured.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a69c-08eb80a9060b', N'Camiseta Rick And Morty', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Rick And Morty.webp', 5)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a79c-08eb80a9060b', N'Camiseta Say My Name', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Say My Name.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a89c-08eb80a9060b', N'Camiseta Support', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'support.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'7d67df76-2d4e-4a47-a99c-08eb80a9060b', N'Camiseta Try Hard', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'Tryhard.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476421e', N'Caneca Joker Wanted', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-joker Wanted.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476422e', N'Caneca Joker', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-Joker.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476423e', N'Caneca Nightmare', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-Nightmare.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476424e', N'Caneca Ozob', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-Ozob.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476425e', N'Caneca Rick and Morty', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-Rick and Morty.jpg', 5)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476426e', N'Caneca Wonder Woman', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-Wonder Woman.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476427e', N'Caneca No Coffee No Code', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca4.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476437e', N'Caneca Batman', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca1--batman.jpg', 5)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476447e', N'Caneca Vegeta', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca1-Vegeta.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476457e', N'Caneca Batman Preta', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-Batman.jpg', 8)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476467e', N'Caneca Big Bang Theory', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-bbt.webp', 0)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476477e', N'Caneca Cogumelo', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-cogumelo.webp', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476487e', N'Caneca Geeks', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-Geeks.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'78162be3-61c4-4959-89d7-5ebfb476497e', N'Caneca Ironman', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca-ironman.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'6ecaaa6b-ad9f-422c-b3bb-6013ec27b4bb', N'Camiseta Debugar Preta', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(75.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'camiseta4.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'6ecaaa6b-ad9f-422c-b3bb-6013ec27c4bb', N'Camiseta Code Life Cinza', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(60.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'camiseta3.jpg', 8)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'52dd696b-0882-4a73-9525-6af55dd416a4', N'Caneca Star Bugs Coffee', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(20.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca1.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'191ddd3e-acd4-4c3b-ae74-8e473993c5da', N'Caneca Programmer Code', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(15.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca2.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'fc184e11-014c-4978-aa10-9eb5e1af369b', N'Camiseta Software Developer', N'Camiseta 100% algodão, resistente a lavagens e altas temperaturas.', 1, CAST(100.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'camiseta1.jpg', 10)
INSERT [dbo].[Produtos] ([Id], [Nome], [Descricao], [Ativo], [Valor], [DataCadastro], [Imagem], [QuantidadeEstoque]) VALUES (N'20e08cd4-2402-4e76-a3c9-a026185b193d', N'Caneca Turn Coffee in Code', N'Caneca de porcelana com impressão térmica de alta resistência.', 1, CAST(20.00 AS Decimal(18, 2)), CAST(N'2019-07-19T00:00:00.0000000' AS DateTime2), N'caneca3.jpg', 10)
GO
INSERT [dbo].[Vouchers] ([Id], [Codigo], [Percentual], [ValorDesconto], [Quantidade], [TipoDesconto], [DataCriacao], [DataUtilizacao], [DataValidade], [Ativo], [Utilizado]) VALUES (N'acffa74e-52a4-4567-a878-72921534d325', N'150-OFF-GERAL', NULL, CAST(150.00 AS Decimal(18, 2)), 48, 1, GETDATE(), NULL, DATEADD("qq", 1, GETDATE()), 1, 0)
INSERT [dbo].[Vouchers] ([Id], [Codigo], [Percentual], [ValorDesconto], [Quantidade], [TipoDesconto], [DataCriacao], [DataUtilizacao], [DataValidade], [Ativo], [Utilizado]) VALUES (N'acffa74e-52a4-4567-a878-72921534d327', N'50-OFF-GERAL', CAST(50.00 AS Decimal(18, 2)), NULL, 40, 0, GETDATE(), NULL, DATEADD("qq", 1, GETDATE()), 1, 0)
INSERT [dbo].[Vouchers] ([Id], [Codigo], [Percentual], [ValorDesconto], [Quantidade], [TipoDesconto], [DataCriacao], [DataUtilizacao], [DataValidade], [Ativo], [Utilizado]) VALUES (N'acffa74e-52a4-4567-a878-72921534d328', N'10-OFF-GERAL', CAST(10.00 AS Decimal(18, 2)), NULL, 42, 0, GETDATE(), NULL, DATEADD("qq", 1, GETDATE()), 1, 0)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 06/08/2023 18:11:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 06/08/2023 18:11:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_Cliente]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IDX_Cliente] ON [dbo].[CarrinhoCliente]
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CarrinhoItens_CarrinhoId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_CarrinhoItens_CarrinhoId] ON [dbo].[CarrinhoItens]
(
	[CarrinhoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Enderecos_ClienteId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Enderecos_ClienteId] ON [dbo].[Enderecos]
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PedidoItems_PedidoId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_PedidoItems_PedidoId] ON [dbo].[PedidoItems]
(
	[PedidoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pedidos_VoucherId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_Pedidos_VoucherId] ON [dbo].[Pedidos]
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transacoes_PagamentoId]    Script Date: 06/08/2023 18:11:19 ******/
CREATE NONCLUSTERED INDEX [IX_Transacoes_PagamentoId] ON [dbo].[Transacoes]
(
	[PagamentoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CarrinhoCliente] ADD  DEFAULT ((0.0)) FOR [Desconto]
GO
ALTER TABLE [dbo].[CarrinhoCliente] ADD  DEFAULT ((0)) FOR [TipoDesconto]
GO
ALTER TABLE [dbo].[CarrinhoCliente] ADD  DEFAULT ('') FOR [VoucherCodigo]
GO
ALTER TABLE [dbo].[CarrinhoCliente] ADD  DEFAULT (CONVERT([bit],(0))) FOR [VoucherUtilizado]
GO
ALTER TABLE [dbo].[Pedidos] ADD  DEFAULT (NEXT VALUE FOR [MinhaSequencia]) FOR [Codigo]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[CarrinhoItens]  WITH CHECK ADD  CONSTRAINT [FK_CarrinhoItens_CarrinhoCliente_CarrinhoId] FOREIGN KEY([CarrinhoId])
REFERENCES [dbo].[CarrinhoCliente] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CarrinhoItens] CHECK CONSTRAINT [FK_CarrinhoItens_CarrinhoCliente_CarrinhoId]
GO
ALTER TABLE [dbo].[Enderecos]  WITH CHECK ADD  CONSTRAINT [FK_Enderecos_Clientes_ClienteId] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Clientes] ([Id])
GO
ALTER TABLE [dbo].[Enderecos] CHECK CONSTRAINT [FK_Enderecos_Clientes_ClienteId]
GO
ALTER TABLE [dbo].[PedidoItems]  WITH CHECK ADD  CONSTRAINT [FK_PedidoItems_Pedidos_PedidoId] FOREIGN KEY([PedidoId])
REFERENCES [dbo].[Pedidos] ([Id])
GO
ALTER TABLE [dbo].[PedidoItems] CHECK CONSTRAINT [FK_PedidoItems_Pedidos_PedidoId]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Vouchers_VoucherId] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Vouchers] ([Id])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Vouchers_VoucherId]
GO
ALTER TABLE [dbo].[Transacoes]  WITH CHECK ADD  CONSTRAINT [FK_Transacoes_Pagamentos_PagamentoId] FOREIGN KEY([PagamentoId])
REFERENCES [dbo].[Pagamentos] ([Id])
GO
ALTER TABLE [dbo].[Transacoes] CHECK CONSTRAINT [FK_Transacoes_Pagamentos_PagamentoId]
GO
USE [master]
GO
ALTER DATABASE [NerdStoreEnterpriseDB] SET  READ_WRITE 
GO
