# How to use OData in a ASP.NET Core 3.1 with NHibernate

This repo contains an ASP.NET Core 3.1 Web Api that uses OData on NHibernate

# Package versions


```xml
<PackageReference Include="FluentNHibernate" Version="2.1.2" />
<PackageReference Include="Microsoft.AspNetCore.OData" Version="7.4.0-beta" />
<PackageReference Include="NHibernate" Version="5.2.5" />
<PackageReference Include="System.Data.SqlClient" Version="4.8.1" />
```
I am using OData 7.4-beta because the asp.net 3.0 changed some behavior on routing, and previous versions of the OData for ASP.NET Core was design with the asp.net 2.*

# Explanation

First to configure OData, I've just configured the service

[Startup.cs#Line 26](https://github.com/AlbertoMonteiro/AspNetCoreWithNHibernateAndOData/blob/cefa477eb0b354d3fca285e4059152fac47d0c15/WebWIthNHibernate/Startup.cs#L26)
```csharp
services.AddOData();
```

Also changed UseEndpoints with the following code

[Startup.cs#Line 57-58](https://github.com/AlbertoMonteiro/AspNetCoreWithNHibernateAndOData/blob/master/WebWIthNHibernate/Startup.cs#L57-L58)
```csharp
endpoints.EnableDependencyInjection();
endpoints.MaxTop(30).Filter().Count().Select().OrderBy();
```

After that I've created a custom version of `EnableQueryAttribute` that I called [EnableQueryCustomAttribute](https://github.com/AlbertoMonteiro/AspNetCoreWithNHibernateAndOData/blob/master/WebWIthNHibernate/Models/EnableQueryCustom.cs#L12-L33) to allow change data structure when `$count=true` is sent in query string, so I can return items with the total count of items in database based on query filters.

So I added this attribute to my action [PessoasController.cs#L23](https://github.com/AlbertoMonteiro/AspNetCoreWithNHibernateAndOData/blob/master/WebWIthNHibernate/Controllers/PessoasController.cs#L23).

Then I'just had to configure NHibernate session that I did in [Startup.cs#L28-L39](https://github.com/AlbertoMonteiro/AspNetCoreWithNHibernateAndOData/blob/master/WebWIthNHibernate/Startup.cs#L28-L39)

Now the easy part, in my action I just use the Query method from session, that gives me an IQueryable that I can give to OData do the query mutations and get my data without any problems!
