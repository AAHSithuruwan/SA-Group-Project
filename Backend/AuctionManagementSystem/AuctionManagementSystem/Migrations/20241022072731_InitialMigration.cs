using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            // Insert data into the Category table
            migrationBuilder.InsertData(
                table: "Categories",
                columns: ["Name"],
                values: new object[,]
                {
                    { "Electronics" },
                    { "Fashion" },
                    { "Home & Garden" },
                    { "Art & Collectibles" },
                    { "Vehicles" },
                    { "Sports Equipment" },
                    { "Jewelry" },
                    { "Toys & Games" },
                    { "Books" },
                    { "Antiques" }
                });


            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    IsAdmin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            // Insert data into the User table
            migrationBuilder.InsertData(
                table: "Users",
                columns: ["Email", "Password", "FirstName", "LastName", "Address", "PhoneNumber", "IsAdmin"],
                values: new object[,]
                {
                    { "user1@gmail.com", "AQAAAAIAAYagAAAAEPAeZvu/MqbtODRykJHbZuS6VCNvR1jiDe9pPvhp02pI5ksmmqX1lh8LZyZX/Gv9Ow==", "Nimal", "Wickramasingha", "Colombo 13", "1234567890", 0 }, // Password : user12345
                    { "user2@gmail.com", "AQAAAAIAAYagAAAAEJ6Tg7pKFvi/KZnZpER61L3gPnBzqJybpB70qYQfwGXMaXVhxbpA8oKScOFl9oR1tg==", "Bloyd", "Dias", "Colombo 10", "1234560890", 0 }, // Password : user12345
                    { "seller1@gmail.com", "AQAAAAIAAYagAAAAED5wIjq65wERi7rfnvuX769MJnR9mgHq4gVOI+ts7pLizKwsCzkqHDYi4Cno30CwxQ==", "Ruwanthi", "Gamage", "Colombo 15", "0987654321", 0 }, // Password : seller12345
                    { "seller2@gmail.com", "AQAAAAIAAYagAAAAEJ20ugcky3vVnCTUoytmgOv7I9gwK2AJnLhqXBmfNAUAoZ91fK8GoBcoG08of8OfkA==", "Peter", "Patrik", "Colombo 9", "0987654331", 0 }, // Password : seller12345
                    { "admin@gmail.com", "AQAAAAIAAYagAAAAEHSH96NfuScv3n8cuNSg6s6imFw9Gt+YzzW8AY146RwnyhKV6/6CxY5DxiZButJogw==", "Tharushi", "Raigama", "Colombo 12", "1122334455", 1 } // Password : admin12345
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    SellerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.SellerId);
                    table.ForeignKey(
                        name: "FK_Sellers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            //Insert data into seller table
            migrationBuilder.InsertData(
            table: "Sellers",
            columns: ["FirstName", "LastName", "Email", "PhoneNumber", "Address", "UserId"],
            values: new object[,]
            {
                { "Ruwanthi", "Gamage", "ruwanthigamage@gmail.com", "1234567890", "Colombo 2", 3 },
                { "Peter", "Patrik", "peterpatrik@gmail.com", "0987654321", "Colombo 11", 4 }
            });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    AuctionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartingPrice = table.Column<float>(type: "real", nullable: false),
                    BidIncrement = table.Column<float>(type: "real", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.AuctionId);
                    table.ForeignKey(
                        name: "FK_Auctions_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "SellerId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Insert data into auction table
            migrationBuilder.InsertData(
                table: "Auctions",
                columns: ["StartingPrice", "BidIncrement", "StartingDate", "EndDate", "SellerId"],
                values: new object[,]
                {
                    // Electronics
                    // Not Started
                    { 70000f, 5000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(9), 1 }, 
                    { 89900f, 5000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(8), 2 },
                    // Ongoing
                    { 250000f, 15000f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(6), 1 }, 
                    { 120000f, 10000f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(2), 2 }, 
                    // Ended
                    { 49900f, 2500f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-1), 1 }, 
                    { 29900f, 2000f, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-2), 2 }, 

                    // Fashion
                    // Not Started
                    { 120000f, 10000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(9), 1 }, 
                    { 80000f, 5000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 2 }, 
                    // Ongoing
                    { 15000f, 1500f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(5), 1 }, 
                    { 1000000f, 50000f, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(3), 2 },
                    // Ended
                    { 35000f, 2500f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-1), 1 },
                    { 90000f, 5000f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-2), 2 }, 

                    // Home & Garden
                    // Not Started
                    { 45000f, 3000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(6), 1 }, 
                    { 65000f, 4000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(9), 2 }, 
                    // Ongoing
                    { 70000f, 4500f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(5), 1 }, 
                    { 30000f, 2000f, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(4), 2 }, 
                    // Ended
                    { 85000f, 5000f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), 1 }, 
                    { 130000f, 7000f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-2), 2 }, 

                    // Art & Collectibles
                    // Not Started
                    { 500000f, 20000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(14), 1 }, 
                    { 150000f, 10000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 2 }, 
                    // Ongoing
                    { 250000f, 15000f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(5), 1 }, 
                    { 350000f, 20000f, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(3), 2 }, 
                    // Ended
                    { 100000f, 7500f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-2), 1 }, 
                    { 5000000f, 250000f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), 2 }, 

                    // Vehicles
                    // Not Started
                    { 7000000f, 500000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(20), 1 }, 
                    { 5500000f, 400000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(18), 2 }, 
                    // Ongoing
                    { 3500000f, 250000f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(15), 1 }, 
                    { 4500000f, 350000f, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(19), 2 }, 
                    // Ended
                    { 8000000f, 600000f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-2), 1 }, 
                    { 3000000f, 200000f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(14), 2 }, 

                    // Sports Equipment
                    // Not Started
                    { 15000f, 1000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(5), 1 }, 
                    { 25000f, 2000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(7), 2 }, 
                    // Ongoing
                    { 220000f, 15000f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(12), 1 }, 
                    { 120000f, 7500f, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(9), 2 }, 
                    // Ended
                    { 18000f, 1500f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), 1 }, 
                    { 30000f, 2500f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-2), 2 }, 

                    // Jewelry
                    // Not Started
                    { 50000f, 5000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(7), 1 }, 
                    { 30000f, 2000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(8), 2 }, 
                    // Ongoing
                    { 20000f, 1500f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(6), 1 }, 
                    { 45000f, 3000f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(10), 2 }, 
                    // Ended
                    { 15000f, 1000f, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-1), 1 }, 
                    { 70000f, 7000f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-2), 2 }, 

                    // Toys & Games
                    // Not Started
                    { 5000f, 500f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(4), 1 }, 
                    { 3000f, 300f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), 2 }, 
                    // Ongoing
                    { 10000f, 1000f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(6), 1 }, 
                    { 8000f, 800f, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(3), 2 }, 
                    // Ended
                    { 2500f, 200f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), 1 }, 
                    { 20000f, 1500f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-2), 2 }, 

                    // Books
                    // Not Started
                    { 2000f, 200f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(4), 1 }, 
                    { 1500f, 150f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), 2 }, 
                    // Ongoing
                    { 3000f, 300f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(5), 1 }, 
                    { 10000f, 500f, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(4), 2 }, 
                    // Ended
                    { 2500f, 250f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), 1 }, 
                    { 3500f, 350f, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2), 2 },

                    // Antiques
                    // Not Started
                    { 500000f, 25000f, DateTime.Now.AddDays(2), DateTime.Now.AddDays(10), 1 }, 
                    { 75000f, 5000f, DateTime.Now.AddDays(1), DateTime.Now.AddDays(8), 2 }, 
                    // Ongoing
                    { 300000f, 15000f, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(5), 1 }, 
                    { 200000f, 10000f, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(7), 2 }, 
                    // Ended
                    { 150000f, 7000f, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-2), 1 }, 
                    { 90000f, 4000f, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-3), 2 },
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    BidId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<float>(type: "real", nullable: false),
                    BidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShippingPhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AuctionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.BidId);
                    table.ForeignKey(
                        name: "FK_Bids_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "AuctionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bids_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDispatched = table.Column<int>(type: "int", nullable: false),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "AuctionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Insert data into product table
            migrationBuilder.InsertData(
                table: "Products",
                columns: ["Name", "Description", "IsDispatched", "AuctionId", "CategoryId"],
                values: new object[,]
                {
                    // Electronics
                    { "iPhone 14", "The latest Apple iPhone 14 features an advanced A15 Bionic chip, ensuring lightning-fast performance for gaming and multitasking. With 256GB of storage, users can store photos, videos, and apps without worrying about space. The stunning Super Retina XDR display delivers vibrant colors and exceptional contrast, making everything from streaming movies to browsing the web a joy.", 0, 1, 1 },
                    { "Samsung Galaxy S23", "The Samsung Galaxy S23 offers cutting-edge technology with a high-resolution AMOLED display that brings images to life. Its 128GB storage capacity allows users to download numerous apps, music, and videos without concern. This smartphone's powerful camera system enables capturing stunning photos in any lighting, making it perfect for photography enthusiasts.", 0, 2, 1 },
                    { "MacBook Pro", "Apple's MacBook Pro 16-inch with the M1 chip is a powerhouse for professionals and creatives alike. It combines stunning visuals with robust performance, making it ideal for video editing, graphic design, and programming. The long battery life ensures users can work all day without needing to recharge, enhancing productivity significantly.", 0, 3, 1 },
                    { "Sony 4K TV", "The 65-inch Sony Bravia 4K Ultra HD Smart TV provides an immersive viewing experience with breathtaking picture quality. With HDR support, colors appear more vibrant and lifelike, perfect for movie nights and gaming sessions. Integrated smart features allow users to access popular streaming services and apps with ease, making entertainment more convenient.", 0, 4, 1 },
                    { "PlayStation 5", "Experience gaming like never before with the Sony PlayStation 5, featuring lightning-fast load times and stunning graphics. The console includes innovative features such as haptic feedback and adaptive triggers, enhancing the immersive experience of gameplay. With a vast library of exclusive titles, gamers can enjoy hours of entertainment across various genres.", 0, 5, 1 },
                    { "Bose Noise Cancelling Headphones", "Bose QuietComfort 45 headphones are designed for audiophiles who crave the best sound experience. Equipped with advanced noise-canceling technology, they effectively block out distractions, allowing users to immerse themselves in their music or podcasts. The comfortable design ensures long listening sessions are enjoyable, making them perfect for travel or work.", 0, 6, 1 },

                    // Fashion
                    { "Gucci Leather Jacket", "This luxury Gucci leather jacket combines style and sophistication, perfect for those who want to make a statement. Crafted from premium leather, it features a classic design with modern touches, ensuring it remains timeless. Whether paired with jeans or dress pants, this jacket elevates any outfit, making it ideal for various occasions.", 0, 7, 2 },
                    { "Prada Handbag", "The Prada designer handbag is a must-have accessory for fashion enthusiasts who appreciate elegance and quality. Made from high-quality materials, this handbag showcases intricate craftsmanship and a stylish design that complements any outfit. Its spacious interior allows for easy organization, making it perfect for daily use or special occasions.", 0, 8, 2 },
                    { "Nike Air Max Sneakers", "Limited edition Nike Air Max running shoes combine style and performance for athletes and sneaker enthusiasts. With advanced cushioning technology, these shoes provide exceptional comfort and support during workouts or casual outings. The trendy design ensures that they look great whether you're at the gym or hanging out with friends.", 0, 9, 2 },
                    { "Rolex Submariner", "The Rolex Submariner dive watch is a symbol of luxury and precision, designed for underwater adventures. Its waterproof design and durable materials make it ideal for divers, while the elegant appearance makes it suitable for formal occasions. This watch combines functionality with timeless style, making it a treasured accessory for any collection.", 0, 10, 2 },
                    { "Louis Vuitton Scarf", "Wrap yourself in luxury with this designer Louis Vuitton silk scarf, perfect for adding a touch of elegance to any outfit. Made from the finest silk, the scarf features a beautiful print that embodies the brand's iconic style. Whether worn as a neck accessory or headpiece, it elevates any look effortlessly.", 0, 11, 2 },
                    { "Armani Suit", "Men's Giorgio Armani tailored suit offers a perfect blend of style and comfort, ideal for business meetings or formal events. The high-quality fabric ensures a perfect fit, while the sophisticated design exudes confidence and professionalism. Pair it with a crisp shirt and tie for a complete ensemble that commands attention.", 0, 12, 2 },

                    // Home & Garden
                    { "IKEA Sofa", "This modern 3-seater sofa from IKEA provides a perfect balance of comfort and style, making it a great addition to any living room. With its sleek design and durable upholstery, it easily fits into contemporary and classic decors alike. The spacious seating allows for relaxation, whether you’re entertaining guests or enjoying a movie night.", 0, 13, 3 },
                    { "Dyson Vacuum Cleaner", "The cordless Dyson V15 Detect vacuum cleaner is equipped with advanced technology to make cleaning effortless. Its powerful suction and smart sensor technology automatically adjust to different floor types, ensuring a deep clean every time. Lightweight and easy to maneuver, this vacuum is perfect for quick clean-ups and thorough deep cleans.", 0, 14, 3 },
                    { "Weber Grill", "The Weber Genesis II gas grill is perfect for outdoor cooking enthusiasts who enjoy hosting barbecues. With its spacious cooking surface and innovative features, grilling meats and vegetables to perfection is easier than ever. The durable design ensures that it can withstand the elements, making it a long-lasting addition to your outdoor space.", 0, 15, 3 },
                    { "Philips Air Fryer", "The Philips XL Airfryer uses rapid air technology to fry food with little to no oil, making it a healthier cooking option. This versatile appliance allows for frying, baking, grilling, and roasting, catering to various culinary needs. Its sleek design and user-friendly interface make it a must-have in any modern kitchen.", 0, 16, 3 },
                    { "Bosch Dishwasher", "The Bosch Series 6 fully integrated dishwasher combines style and efficiency, designed to seamlessly fit into your kitchen cabinetry. With multiple wash programs and excellent energy efficiency, it ensures sparkling clean dishes every time. Its quiet operation and sleek design make it a great addition to any home, enhancing your cooking experience.", 0, 17, 3 },
                    { "Samsung Refrigerator", "The Samsung Family Hub French door refrigerator features a smart touchscreen that allows you to manage your groceries with ease. With ample storage and innovative features, it keeps your food fresh while allowing you to create shopping lists and schedule family events. This refrigerator combines technology and style, making it a centerpiece in your kitchen.", 0, 18, 3 },

                    // Art & Collectibles
                    { "Vincent van Gogh Print", "Reproduction of 'Starry Night' by Van Gogh offers art lovers a chance to enjoy a piece of history. This stunning print captures the vibrant colors and swirling patterns of the original painting, bringing beauty to any space. Whether hung in a living room or an office, it serves as a source of inspiration and admiration.", 0, 19, 4 },
                    { "Antique Vase", "This exquisite 19th-century porcelain vase is a rare collectible that adds elegance to any decor. Its intricate design and fine craftsmanship showcase the artistry of the period, making it a treasured piece for collectors. Display it prominently to create a focal point in your home or as part of a curated collection.", 0, 20, 4 },
                    { "Signed Football", "This signed football by Lionel Messi is a must-have for sports enthusiasts and collectors alike. Its authenticity guarantees a unique piece of memorabilia from one of the greatest football players in history. Display it in your home or office to celebrate your love for the game and inspire future generations.", 0, 21, 4 },
                    { "Rare Coin Set", "Limited edition coin set from the 1800s offers a glimpse into history for numismatists and collectors. Each coin is carefully preserved, showcasing the artistry and craftsmanship of its time. This set serves as a valuable investment and a fascinating conversation piece for anyone interested in historical artifacts.", 0, 22, 4 },
                    { "Autographed Beatles Vinyl", "This autographed copy of a Beatles record is a treasure for music lovers and collectors. Featuring the signatures of the legendary band members, it captures a moment in music history. Frame it for display or add it to your collection to celebrate the timeless legacy of The Beatles.", 0, 23, 4 },
                    { "Original Picasso Sketch", "Authentic sketch by Pablo Picasso provides a unique opportunity to own a piece of art history. This original work showcases Picasso's distinctive style, making it a valuable addition to any collection. Whether displayed in a gallery or home, it serves as a captivating conversation starter about the world of modern art.", 0, 24, 4 },

                    // Vehicles
                    { "Tesla Model S", "The 2022 Tesla Model S electric sedan offers cutting-edge technology and impressive performance for eco-conscious drivers. With a sleek design and spacious interior, it combines luxury with sustainability, making it ideal for modern lifestyles. The advanced autopilot feature enhances safety and convenience, allowing for a more enjoyable driving experience.", 0, 25, 5 },
                    { "Harley Davidson Motorcycle", "This classic Harley Davidson cruiser is perfect for motorcycle enthusiasts seeking adventure on the open road. With its powerful engine and iconic design, it offers an exhilarating ride that turns heads wherever you go. Whether you're cruising through the city or exploring the countryside, this bike promises an unforgettable experience.", 0, 26, 5 },
                    { "Toyota Camry", "The Toyota Camry is a reliable and efficient sedan, ideal for daily commuting or family outings. With a spacious interior and advanced safety features, it provides comfort and peace of mind on the road. Its fuel-efficient engine and smooth handling make it a popular choice for drivers seeking value and performance.", 0, 27, 5 },
                    { "Ford F-150", "The Ford F-150 is a versatile pickup truck designed for work and play, offering exceptional towing and hauling capabilities. Its rugged construction and powerful engine make it suitable for various terrains and tasks. With modern technology and comfort features, this truck is perfect for outdoor enthusiasts and professionals alike.", 0, 28, 5 },
                    { "Chevrolet Corvette", "The Chevrolet Corvette is an iconic sports car that delivers thrilling performance and head-turning design. With its powerful engine and lightweight construction, it offers an exhilarating driving experience. Whether you're on the racetrack or cruising down the highway, the Corvette combines speed and style in a way few can match.", 0, 29, 5 },
                    { "Honda Civic", "The Honda Civic is a compact car known for its reliability and fuel efficiency, making it an excellent choice for budget-conscious drivers. With a sleek design and advanced technology features, it offers a comfortable ride for daily commuting. Its spacious interior and ample cargo space add to its appeal for families and individuals alike.", 0, 30, 5 },

                    // Sports Equipment
                    { "Wilson Tennis Racket", "The Wilson Pro Staff tennis racket offers professional-level performance for players at all skill levels. Its lightweight design and advanced technology ensure superior control and power on the court. Whether practicing your serve or competing in a tournament, this racket helps elevate your game and improve your performance.", 0, 31, 6 },
                    { "Adidas Soccer Ball", "The Adidas Tango soccer ball is perfect for training sessions and matches, known for its durability and precision. With its classic design and excellent grip, it allows players to control the ball effortlessly. Whether you're a beginner or a seasoned player, this soccer ball enhances your game experience.", 0, 32, 6 },
                    { "Nike Running Shoes", "The Nike Zoom Fly running shoes are designed for performance and comfort, ideal for long-distance runners. With responsive cushioning and a lightweight design, they provide support during your workouts. Available in various colors, these shoes are as stylish as they are functional, making them a great addition to any athlete's gear.", 0, 33, 6 },
                    { "Under Armour Gym Bag", "The Under Armour gym bag offers ample storage for all your workout essentials, combining style and functionality. Its durable materials ensure it withstands the rigors of daily use, while multiple pockets help organize your gear. Whether heading to the gym or traveling, this bag is a reliable companion for active lifestyles.", 0, 34, 6 },
                    { "Trek Marlin 7 Mountain Bike", "The Trek Marlin 7 mountain bike is designed for both casual riders and serious enthusiasts, featuring a lightweight frame and high-quality components. With excellent suspension and responsive handling, it ensures a smooth ride on various terrains. This bike is perfect for outdoor adventures and exploring new trails with friends and family.", 0, 35, 6 },
                    { "Yoga Mat", "The Liforme yoga mat provides superior grip and comfort for your practice, making it an essential accessory for yoga enthusiasts. Made from eco-friendly materials, it offers excellent durability and cushioning for various poses. Its non-slip surface ensures stability, allowing you to focus on your practice and achieve your fitness goals.", 0, 36, 6 },

                    // Jewelry
                    { "Diamond Engagement Ring", "This exquisite diamond engagement ring features a stunning center stone set in a timeless band, perfect for proposing to your loved one. Crafted from high-quality materials, its brilliance and elegance symbolize eternal love and commitment. It's the ideal way to express your feelings and celebrate your special moment together.", 0, 37, 7 },
                    { "Gold Necklace", "This elegant 14K gold necklace is a versatile accessory that complements any outfit, from casual to formal. Its delicate design and lightweight construction make it comfortable for everyday wear. Whether as a gift or a personal treat, this necklace is a timeless addition to any jewelry collection.", 0, 38, 7 },
                    { "Silver Bracelet", "This beautiful sterling silver bracelet features intricate designs, adding a touch of elegance to any look. With its adjustable length, it fits comfortably on any wrist and can be worn alone or stacked with other bracelets. Perfect for daily wear or special occasions, it enhances your style effortlessly.", 0, 39, 7 },
                    { "Pearl Earrings", "These classic pearl stud earrings are a must-have accessory for anyone who appreciates timeless elegance. Made from high-quality freshwater pearls, they add a touch of sophistication to any outfit. Perfect for formal events or everyday wear, these earrings are a versatile addition to your jewelry collection.", 0, 40, 7 },
                    { "Tennis Bracelet", "This stunning diamond tennis bracelet features a continuous line of sparkling diamonds, perfect for special occasions or adding luxury to everyday wear. Its flexible design ensures a comfortable fit on any wrist, making it easy to wear all day. This bracelet is a timeless piece that enhances any jewelry collection.", 0, 41, 7 },
                    { "Sapphire Pendant", "This stunning sapphire pendant necklace features a brilliant blue gemstone elegantly set in a delicate silver or gold setting. Its timeless design makes it a perfect accessory for both casual and formal occasions. Whether worn alone or layered with other necklaces, this pendant adds a touch of sophistication and charm to any outfit.", 0, 42, 7 },

                    // Toys & Games
                    { "LEGO Star Wars Set", "This LEGO Star Wars set offers hours of imaginative play for fans of all ages, allowing them to build iconic spacecraft and recreate epic battles. With detailed mini-figures and accessories, it provides a comprehensive experience for building and storytelling. Perfect for kids and collectors, it enhances creativity and fine motor skills.", 0, 43, 8 },
                    { "Barbie Dreamhouse", "The Barbie Dreamhouse playset is a dream come true for young children, providing endless opportunities for imaginative play. Featuring multiple rooms, furniture, and accessories, it allows kids to create their own stories and adventures. This colorful and detailed house encourages creativity, role-playing, and social interaction among friends.", 0, 44, 8 },
                    { "Monopoly Board Game", "Monopoly is a classic board game that combines strategy and fun, perfect for family game nights. Players buy, sell, and trade properties while trying to outsmart their opponents to become the wealthiest player. This timeless game fosters critical thinking, negotiation skills, and friendly competition, making it a favorite for all ages.", 0, 45, 8 },
                    { "Action Figures", "This set of superhero action figures is ideal for kids who love adventure and imaginative play. Featuring their favorite characters, these figures inspire storytelling and creative scenarios. Perfect for solo play or group activities, they help develop social skills and creativity while keeping children entertained for hours.", 0, 46, 8 },
                    { "1000-piece jigsaw puzzle", "This 1000-piece jigsaw puzzle offers a fun and challenging activity for families and friends, providing a great way to bond and relax. With vibrant artwork and intricate designs, it stimulates the mind and encourages teamwork as participants work together to complete the picture. Perfect for rainy days or cozy evenings at home.", 0, 47, 8 },
                    { "Dolls House", "This beautifully crafted dolls house provides a world of creativity and imagination for children to explore. With multiple rooms and detailed accessories, it encourages storytelling and role-play, making it perfect for interactive play. Durable and charming, this dolls house will be a treasured part of childhood memories.", 0, 48, 8 },

                    // Books
                    { "To Kill a Mockingbird", "Harper Lee's classic novel 'To Kill a Mockingbird' explores themes of racial injustice and moral growth in the American South. Told through the eyes of young Scout Finch, the story captivates readers with its poignant storytelling and unforgettable characters. This timeless book is a must-read for anyone interested in literature and social issues.", 0, 49, 9 },
                    { "1984 by George Orwell", "George Orwell's dystopian novel '1984' delves into the dangers of totalitarianism and the loss of individuality. Set in a bleak future where surveillance and propaganda are rampant, it serves as a cautionary tale for readers. This thought-provoking book remains relevant today, inviting discussions about freedom and government control.", 0, 50, 9 },
                    { "Pride and Prejudice", "Jane Austen's 'Pride and Prejudice' is a beloved romance that captures the complexities of love and societal expectations in the early 19th century. The witty dialogue and sharp observations make it a delightful read. This novel continues to resonate with readers, highlighting themes of class, marriage, and individuality.", 0, 51, 9 },
                    { "The Great Gatsby", "F. Scott Fitzgerald's 'The Great Gatsby' is a quintessential American novel that explores themes of wealth, love, and the American Dream. Set in the Jazz Age, it offers a glimpse into the excess and disillusionment of the time. This beautifully written book remains a classic, captivating readers with its rich symbolism and unforgettable characters.", 0, 52, 9 },
                    { "Harry Potter and the Philosopher's Stone", "J.K. Rowling's 'Harry Potter and the Philosopher's Stone' introduces readers to the magical world of Hogwarts. Following the adventures of Harry Potter and his friends, it combines themes of friendship, bravery, and the battle between good and evil. This enchanting book has become a global phenomenon, inspiring generations of readers.", 0, 53, 9 },
                    { "The Alchemist", "Paulo Coelho's 'The Alchemist' is a philosophical novel that follows a shepherd named Santiago on his journey to discover his personal legend. This inspiring tale encourages readers to pursue their dreams and listen to their hearts. Its universal themes and beautiful prose make it a timeless classic cherished by many.", 0, 54, 9 },

                    // Antiques
                    { "Victorian Style Chair", "This exquisite Victorian-style chair features intricate woodwork and luxurious upholstery, embodying the elegance of the era. Perfect for collectors and enthusiasts, it adds a touch of history and sophistication to any room. Display it as a statement piece in your home or office to create a classic atmosphere.", 0, 55, 10 },
                    { "Vintage Watch", "This vintage pocket watch from the 1920s is a stunning collectible that reflects the craftsmanship of a bygone era. Its intricate design and precise mechanics make it a remarkable piece for watch enthusiasts and collectors alike. Display it as a unique conversation starter or wear it as a reminder of timeless elegance.", 0, 56, 10 },
                    { "Antique Silver Cutlery", "This set of antique silver cutlery, dating back to the 19th century, is perfect for special occasions and collectors. Featuring intricate designs and fine craftsmanship, it adds elegance to any dining table. Cherished for its beauty and history, this cutlery set is a valuable addition to any collection.", 0, 57, 10 },
                    { "Old Maps", "This collection of antique maps from the 18th century provides a fascinating glimpse into history and exploration. Each map showcases the artistry and craftsmanship of the period, making it a unique decorative piece. Ideal for framing or displaying in a collection, these maps inspire curiosity about the world and its history.", 0, 58, 10 },
                    { "Art Deco Clock", "This Art Deco clock from the 1920s combines beauty and functionality, making it a stunning decorative piece for any home. With its unique geometric design and exquisite materials, it represents the elegance of the Art Deco movement. Perfect for collectors or as a statement piece, it enhances any room's decor.", 0, 59, 10 },
                    { "Antique Trunk", "This beautiful antique trunk from the early 1900s is both functional and decorative, adding character to any space. With its intricate designs and sturdy construction, it serves as a reminder of the past while providing practical storage. Perfect for use as a coffee table or for displaying cherished items, this trunk is a timeless treasure.", 0, 60, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_SellerId",
                table: "Auctions",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionId",
                table: "Bids",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_UserId",
                table: "Bids",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuctionId",
                table: "Products",
                column: "AuctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
