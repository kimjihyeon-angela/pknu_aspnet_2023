using aspnet02_boardapp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace aspnet02_boardapp
{
    public class Program
    {
        // ASP.Net ������ ���� ���� �ʱ�ȭ
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Data���� ���� ApplicationDBContext�� ����ϰڴٴ� ���� �߰�
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                // appsettings.json ConnectionStrings���� ���� ���ڿ� �Ҵ�
                builder.Configuration.GetConnectionString("DefaultConnection"),
                // ���� ���ڿ��� DB�� ���� ������ �ڵ����� �������� ���� �ǹ���
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                ));

            // 2. ASP.Net Identity - ASP.Net ������ ���� ���� ���� (�߰�)
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ��й�ȣ ��å ���� ����
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Custom Password policy 
                options.Password.RequireDigit = false;           // ������ �ʿ� ����
                options.Password.RequireLowercase = false;       // �ҹ��� �ʿ� ����
                options.Password.RequireUppercase = false;       // �빮�� �ʿ� ����
                options.Password.RequiredLength = 4;             // �ּ� ���� ���� ��
                options.Password.RequireNonAlphanumeric = false; // Ư������ �ʿ� ����
                options.Password.RequiredUniqueChars = 0;        // ��ȣ �������� ��
            });


            // ������ ���� ���� �߰�
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // 3. ASP.Net Identity - ���� (�߰�)

            app.UseAuthorization();  // ����

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}