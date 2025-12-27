Add-Migration InitialCreateUser -Context UserDbContext
Update-Database -Context UserDbContext
Remove-Migration -Context NotificationDbContext
Drop-Database