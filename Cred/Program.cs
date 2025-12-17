using Cred.dbcontex;
using Cred.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    // Suppress PendingModelChanges warning (fixes migration issues)
    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employees}/{action=Index}/{id?}");

// ===== SEED DATABASE WITH SAMPLE DATA =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Add sample employees if none exist
        if (!context.Employees.Any())
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeNo = "E001",
                    Name = "John Doe",
                    DOB = new DateTime(1980, 1, 15),
                    Address = "123 Main St, Springfield",
                    PlaceOfBirth = "Springfield"
                },
                new Employee
                {
                    EmployeeNo = "E002",
                    Name = "Jane Smith",
                    DOB = new DateTime(1985, 5, 22),
                    Address = "456 Elm St, Shelbyville",
                    PlaceOfBirth = "Shelbyville"
                },
                new Employee
                {
                    EmployeeNo = "E003",
                    Name = "Bob Johnson",
                    DOB = new DateTime(1990, 9, 10),
                    Address = "789 Oak St, Capital City",
                    PlaceOfBirth = "Capital City"
                }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();
            Console.WriteLine("Sample employees created successfully!");

            // Add sample employment records
            var employments = new List<PreviousEmployment>
            {
                new PreviousEmployment
                {
                    EmployeeId = 1,
                    CompanyName = "ABC Corp",
                    JobTitle = "Manager",
                    FromDate = new DateTime(2015, 3, 20),
                    ToDate = new DateTime(2018, 5, 22)
                },
                new PreviousEmployment
                {
                    EmployeeId = 2,
                    CompanyName = "XYZ Inc",
                    JobTitle = "Analyst",
                    FromDate = new DateTime(2011, 1, 10),
                    ToDate = new DateTime(2016, 4, 15)
                },
                new PreviousEmployment
                {
                    EmployeeId = 3,
                    CompanyName = "Tech Solutions",
                    JobTitle = "Developer",
                    FromDate = new DateTime(2017, 6, 24),
                    ToDate = new DateTime(2020, 9, 10)
                }
            };

            context.PreviousEmployments.AddRange(employments);
            context.SaveChanges();
            Console.WriteLine("Sample employment records created successfully!");
        }
        else
        {
            Console.WriteLine("Database already contains data - skipping seed.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding database: {ex.Message}");
    }
}

app.Run();
