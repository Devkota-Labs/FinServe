Add-Migration InitialCreateUser -Context UserDbContext
Update-Database -Context UserDbContext
Remove-Migration
Drop-Database