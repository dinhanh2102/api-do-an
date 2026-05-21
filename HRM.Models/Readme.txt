----Lệnh build database
dotnet ef --startup-project ../ToolManage.Api/ migrations add CreateDatabase

---Lệnh update database
Scaffold-DbContext "Server=14.248.84.128,1445;Database=HRM2AI;User Id=qlnshh;Password=123@qlns;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -StartupProject "HRM.Models" -f -UseDatabaseNames -NoPluralize -Context CoreProjectContext