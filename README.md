# MitsAuth

This repository contains test security projects using IdentityServer4 and ASP.NET Identity


## Projects

### Auth.Level01
This project contains basic ASP.NET MVC app with ASP.NET Identity
Reference: [ASP.NET Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)

**Goal:**
1. Basic Login and Logout
1. Protect a page
2. Management of Users and Roles

**Dependencies**
```
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="5.0.12" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="5.0.12" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="5.0.12" />
```

**Add DbContext**
```
public class ApplicationDbContext : IdentityDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}
}
```

**Configure Connection String**
```
"ConnectionStrings": {
	"DefaultConnection": "Server=DSCLAB-PC;Database=mitsauth-level01db;Trusted_Connection=True;MultipleActiveResultSets=true"
},
```

**Initial Migration to Database Command**
```
Add-Migration -Name "InitialDatabase" -Context ApplicationDbContext -OutputDir Data/Migrations
```

**Update Database Command**
```
Update-Database
```	