# CurrecyReader
## Introduction
The Currency Reader is an application that reads, stores and shows data from https://www.cnb.cz. The Currency Reader consists from three projects:
- CurrencyReader.Service
- CurrencyReader.Data
- CurrencyReader.Web
## CurrencyReader.Service
The CurrencyReader.Service is a .NET Core Console Application which send, parse and store results from requests to https://www.cnb.cz.
To create a data base and fill data follow these steps:
- Launch the CurrencyReader.Service. Data base structure will be initialized.
- To fill the data base execute script [DataScript.sql](DataScript.sql) in the respository.
## CurrencyReader.Data
The CurrencyReader.Data is a .NET Core Library that use Entity Framework as ORM library to access and store a data to a data base.
## CurrencyReader.Web
The CurrencyReader.Web is a .NET Core Web API + Angular Application for output data about exchange of Czech currency.