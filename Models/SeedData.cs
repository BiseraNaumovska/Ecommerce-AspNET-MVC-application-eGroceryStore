using GradinataNaBabaRatka.Areas.Identity.Data;
using GradinataNaBabaRatka.Data;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GradinataNaBabaRatka.Models
{
    public class SeedData
    {
       public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<GradinataNaBabaRatkaUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            GradinataNaBabaRatkaUser user = await UserManager.FindByEmailAsync("admin@gradina.com");
            if (user == null)
            {
                var User = new GradinataNaBabaRatkaUser();
                User.Email = "admin@gradina.com";
                User.UserName = "admin@gradina.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            var roleCheck2 = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck2) { roleResult = await RoleManager.CreateAsync(new IdentityRole("User")); }
            GradinataNaBabaRatkaUser user2 = await UserManager.FindByEmailAsync("defuser@gradina.com");
            if (user2 == null)
            {
                var User = new GradinataNaBabaRatkaUser();
                User.Email = "defuser@gradina.com";
                User.UserName = "defuser@gradina.com";
                string userPWD = "User1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(User, "User");
                }
            }
        }
      
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GradinataNaBabaRatkaContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<GradinataNaBabaRatkaContext>>()))
            {
    //            CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.Proizvod.Any() || context.Prodavac.Any() || context.Grad.Any() || context.Review.Any() || context.Kupuvac.Any())
                {
                    return; // DB has been seeded
                }
                context.Prodavac.AddRange(
                    new Prodavac { /*Id = 1, */FirstName = "Kocho", LastName = "Stanimirov", BirthDate = DateTime.Parse("1999-9-20"), Nationality = "Makedonec", Gender = "Male" },
                    new Prodavac { /*Id = 2, */FirstName = "Gane", LastName = "Janev", BirthDate = DateTime.Parse("1988-8-1"), Nationality = "Makedonec", Gender = "Male" },
                    new Prodavac { /*Id = 3, */FirstName = "Petre", LastName = "Janevski", BirthDate = DateTime.Parse("1998-10-8"), Nationality = "Makedonec", Gender = "Male" },
                    new Prodavac { /*Id = 3, */FirstName = "Olivera", LastName = "Peperova", BirthDate = DateTime.Parse("1955-10-8"), Nationality = "Makedonec", Gender = "Female" }
                );
                context.SaveChanges();
                context.Grad.AddRange(
                    new Grad { /*Id=1 , */ GradIme = "Skopje" },
                    new Grad { /*Id=2 , */ GradIme = "Kumanovo" },
                    new Grad { /*Id=3 , */ GradIme = "Ohrid" },
                    new Grad { /*Id=4 , */ GradIme = "Strumica" },
                    new Grad { /*Id=5 , */ GradIme = "Veles" },
                    new Grad { /*Id=6 , */ GradIme = "Prilep" },
                    new Grad { /*Id=7 , */ GradIme = "Kochani" },
                    new Grad { /*Id=8 , */ GradIme = "Debar" },
                    new Grad { /*Id=9 , */ GradIme = "Gostivar" }
                );
                context.SaveChanges();
                context.Proizvod.AddRange(
                   new Proizvod
                   {
                       //Id = 1,
                       Ime = "Patlidzani",
                       Data = 2023,
                       Zaliha = 300,
                       Opis = "Svez patlidzan, domashen, crven, neprskan",      
                       Slika = "slika1.jpg",
                       Cena = 100,
                       ProdavacId = 1
                   }, new Proizvod
                   {
                       //Id = 1,
                       Ime = "Krastavici",
                       Data = 2023,
                       Zaliha = 250,
                       Opis = "Domashni krastavici za salata",
                       Slika = "slika2.jpg",
                       Cena = 65,
                       ProdavacId = 2
                   },
                    new Proizvod
                    {
                        //Id = 1,
                        Ime = "Oriz",
                        Data = 2024,
                        Zaliha = 500,
                        Opis = "Oriz od ovaa godina",
                        Slika = "slika3.jpg",
                        Cena = 100,
                        ProdavacId = 3
                    },
                     new Proizvod
                     {
                         //Id = 1,
                         Ime = "Kompiri",
                         Data = 2023,
                         Zaliha = 100,
                         Opis = "Star lanski kompir ",
                         Slika = "slika4.jpg",
                         Cena = 40,
                         ProdavacId = 1
                     },
                      new Proizvod
                      {
                          //Id = 1,
                          Ime = "Cveklo",
                          Data = 2023,
                          Zaliha = 45,
                          Opis = "Cveklo pogodno za salata i sokovi",
                          Slika = "slika5.jpg",
                          Cena = 75,
                          ProdavacId = 2
                      },
                       new Proizvod
                       {
                           //Id = 1,
                           Ime = "Morkovi",
                           Data = 2024,
                           Zaliha = 150,
                           Opis = "Morkovi za salata, supa ili pak nekoj vkusen ruchek",
                           Slika = "slika6.jpg",
                           Cena = 35,
                           ProdavacId = 3
                       },
                        new Proizvod
                        {
                            //Id = 1,
                            Ime = "Selski Jajca",
                            Data = 2024,
                            Zaliha = 250,
                            Opis = "Sekoja nedela ima svezi, domashni i selski jajca od kokoshka, guska, patka i noj.",
                            Slika = "slika7.jpg",
                            Cena = 12,
                            ProdavacId = 1
                        }
               );

                context.SaveChanges();

                context.Review.AddRange(
                     new Review
                     {
                         /*Id = 1, */
                         ProizvodId = 1,
                         AppUser = "dejana123",
                         Comment = "Mnogu dobri patlidzani. Sleden pat ke si zemam i za salca! ",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 2, */
                         ProizvodId = 6,
                         AppUser = "bibibibibibi",
                         Comment = "Mi stignaa skapani morkovi. Ama gi iskoristiv za zajcite doma",
                         Rating = 2
                     },
                     new Review
                     {
                         /*Id = 3, */
                         ProizvodId = 4,
                         AppUser = "sara@hotmail.com",
                         Comment = "Dobar kompir, malce so crvi, ama okej e.",
                         Rating = 8
                     },
                     new Review
                     {
                         /*Id = 4, */
                         ProizvodId = 1,
                         AppUser = "simona456",
                         Comment = "Predobri. Ne se dvoumete da zemete",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 5, */
                         ProizvodId = 2,
                         AppUser = "Batak",
                         Comment = "Dobri krastavici, mali, ama mnogu dobri",
                         Rating = 9
                     },
                     new Review
                     {
                         /*Id = 6, */
                         ProizvodId = 3,
                         AppUser = "David_Atanasovski",
                         Comment = " Mnogu vkusen i krupen oriz.",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 7, */
                         ProizvodId = 4,
                         AppUser = "patriot2000",
                         Comment = "Dobar kompir, ama so crvchinja.",
                         Rating = 8
                     },
                     new Review
                     {
                         /*Id = 8, */
                         ProizvodId = 5,
                         AppUser = "JanaBanana",
                         Comment = "Staro cveklo",
                         Rating = 6
                     },
                     new Review
                     {
                         /*Id = 9, */
                         ProizvodId = 4,
                         AppUser = "Velimir",
                         Comment = "dobar e",
                         Rating = 8
                     },
                     new Review
                     {
                         /*Id = 10, */
                         ProizvodId = 1,
                         AppUser = "Danielllll",
                         Comment = "Pak ke kupam definitivno !!!",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 11, */
                         ProizvodId = 2,
                         AppUser = "Tomche",
                         Comment = "uzas !!!",
                         Rating = 2
                     },
                     new Review
                     {
                         /*Id = 12, */
                         ProizvodId = 4,
                         AppUser = "_love_",
                         Comment = "Crvljosan. Mnogu fira ide pri chistenje",
                         Rating = 4
                     },
                     new Review
                     {
                         /*Id = 13, */
                         ProizvodId = 5,
                         AppUser = "baby_girl",
                         Comment = "suvo e...",
                         Rating = 1
                     },
                     new Review
                     {
                         /*Id = 14, */
                         ProizvodId = 7,
                         AppUser = "batman2205",
                         Comment = " 2 od jajcata bea so malo pilence. Nekoja od kokoshkite izgleda gi lezela hahaha ",
                         Rating = 3
                     }
                 );
                context.SaveChanges();

                context.Kupuvac.AddRange(
                    new Kupuvac {/*Id = 1*/AppUser = "_love_", ProizvodId = 4 },
                    new Kupuvac {/*Id = 1*/AppUser = "batman2205", ProizvodId = 7 },
                    new Kupuvac {/*Id = 1*/AppUser = "baby_girl", ProizvodId = 5 }
                    );
                context.SaveChanges();

                context.ProizvodGrad.AddRange(
                new ProizvodGrad { ProizvodId = 1, GradId = 1 },
                new ProizvodGrad { ProizvodId = 1, GradId = 9 },
                new ProizvodGrad { ProizvodId = 1, GradId = 2 },
                new ProizvodGrad { ProizvodId = 1, GradId = 3 },
                new ProizvodGrad { ProizvodId = 2, GradId = 1 },
                new ProizvodGrad { ProizvodId = 2, GradId = 9 },
                new ProizvodGrad { ProizvodId = 2, GradId = 2 },
                new ProizvodGrad { ProizvodId = 2, GradId = 3 },
                new ProizvodGrad { ProizvodId = 3, GradId = 1 },
                new ProizvodGrad { ProizvodId = 3, GradId = 9 },
                new ProizvodGrad { ProizvodId = 3, GradId = 2 },
                new ProizvodGrad { ProizvodId = 3, GradId = 3 },
                new ProizvodGrad { ProizvodId = 4, GradId = 3 },
                new ProizvodGrad { ProizvodId = 4, GradId = 5 },
                new ProizvodGrad { ProizvodId = 4, GradId = 2 },
                new ProizvodGrad { ProizvodId = 5, GradId = 3 },
                new ProizvodGrad { ProizvodId = 5, GradId = 5 },
                new ProizvodGrad { ProizvodId = 5, GradId = 2 },
                new ProizvodGrad { ProizvodId = 6, GradId = 3 },
                new ProizvodGrad { ProizvodId = 6, GradId = 5 },
                new ProizvodGrad { ProizvodId = 6, GradId = 2 },
                new ProizvodGrad { ProizvodId = 7, GradId = 4 },
                new ProizvodGrad { ProizvodId = 7, GradId = 5 },
                new ProizvodGrad { ProizvodId = 7, GradId = 1 },
                new ProizvodGrad { ProizvodId = 8, GradId = 4 },
                new ProizvodGrad { ProizvodId = 8, GradId = 5 },
                new ProizvodGrad { ProizvodId = 8, GradId = 1 }
                );
                context.SaveChanges();
            }
        }
    }
}
