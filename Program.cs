using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Collections.Generic;


namespace Restaurant
{
    
    class Stock
    {
        public string id { get; set; }
        public string name { get; set; }
        public int portionCount { get; set; }
        public string unit { get; set; }
        public string portionSize { get; set; }

        public override string ToString()
        {
            return String.Format("{0,6} | {1,15} | {2,15} | {3,10} | {4,15}", id, name, portionCount, unit, portionSize);
        }

    }

    class Menu
    {
        public string id { get; set; }
        public string name { get; set; }
        public string products { get; set; }

        public override string ToString()
        {

            return String.Format("{0,6} | {1,25} | {2,15}", id, name, products);

        }

    }

    class Order
    {
        public string id { get; set; }
        public string dateTime { get; set; }
        public string items { get; set; }

        public override string ToString()
        {
            return String.Format("{0,6} | {1,25} | {2,15}", id, dateTime, items);
        }

    }



    class Program
    {
        
        List<Stock> ReadAllStock(bool write)
        {
            
            List<Stock> Stocks = new List<Stock>();

            using (var reader = new StreamReader(@"../../../data/Stock.csv"))
            {
                var line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var values = line.Split(',');

                    Stocks.Add(new Stock() { id = values[0], name = values[1], portionCount = int.Parse(values[2]), unit = values[3], portionSize = values[4] });

                }
            }

            if(write == true)
            {
                Console.WriteLine(String.Format("{0,6} | {1,15} | {2,15} | {3,10} | {4,15}", "Id", "Name", "Portion Count", "Unit", "Portion Size"));
                foreach (Stock x in Stocks)
                {
                    Console.WriteLine("------------------------------------------------------------------------------");
                    Console.WriteLine(x.ToString());
                }
            }
            

            return Stocks;
        }

        List<Menu> ReadAllMenu(bool write)
        {

            List<Menu> Menus = new List<Menu>();

            using (var reader = new StreamReader(@"../../../data/Menu.csv"))
            {
                var line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var values = line.Split(',');

                    Menus.Add(new Menu() { id = values[0], name = values[1], products = values[2] });

                }
            }
            
            if(write == true)
            {
                Console.WriteLine(String.Format("{0,6} | {1,25} | {2,15}", "Id", "Name", "Products"));
                foreach (Menu x in Menus)
                {
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine(x.ToString());
                }
            }
            

            return Menus;
        }



        List<Order> ReadAllOrders(bool write)
        {

            List<Order> Orders = new List<Order>();

            using (var reader = new StreamReader(@"../../../data/Orders.csv"))
            {
                var line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var values = line.Split(',');

                    Orders.Add(new Order() { id = values[0], dateTime = values[1], items = values[2] });

                }
            }
            if (write == true)
            {
                Console.WriteLine(String.Format("{0,6} | {1,25} | {2,15}", "Id", "DateTime", "Menu Items"));
                foreach (Order x in Orders)
                {
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine(x.ToString());
                }
            }
            

            return Orders;
        }


        void addLine(string line, string path)
        {

            using (var writer = new StreamWriter(@path, true))
            {
                writer.WriteLine(line);
            }
        }

        int getLinesCount(string path)
        {
            int count = 0;
            string x = "";
            using (StreamReader r = new StreamReader(@path))
            {
                
                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    count++;
                    x = line;
                }

                if (count > 1)
                {
                    string[] c = x.Split(',');
                    count = int.Parse(c[0]) + 1;
                }
            }

            
            
            return count;
        }

        bool deductFromStock(string meals)
        {

            var mealsValues = meals.Split(' ');
            List<Menu> menuTmp = ReadAllMenu(false);
            List<Stock> stockTmp = ReadAllStock(false);
            int totalProductsCount = 0;
            int howManyDone = 0;

            for (int i = 0; i < mealsValues.Length; i++)
            {
                bool foundAMeal = false;
                foreach(Menu x in menuTmp)
                {
                    if(x.id == mealsValues[i])
                    {
                        foundAMeal = true;
                        var productsValues = x.products.Split(' ');
                        totalProductsCount = totalProductsCount + productsValues.Length;

                        for (int j = 0; j < stockTmp.Count; j++)
                        {

                            for(int b = 0; b < productsValues.Length; b++)
                            {

                                if(stockTmp[j].id == productsValues[b])
                                {

                                    if (stockTmp[j].portionCount > 1)
                                    {
                                        stockTmp[j].portionCount = stockTmp[j].portionCount - 1;
                                        howManyDone++;
                                    }
                                        

                                }

                            }

                        }



                    }
                }
                if(foundAMeal == false)
                {
                    return false;
                }

            }

            if(howManyDone == totalProductsCount)
            {

                using (StreamWriter w = new StreamWriter(@"../../../data/Stock.csv"))
                {
                    string line = string.Format("{0},{1},{2},{3},{4}", "Id", "Name", "Portion Count", "Unit", "Portion size");
                    w.WriteLine(line);
                    foreach (Stock s in stockTmp)
                    {
                        line = string.Format("{0},{1},{2},{3},{4}", s.id, s.name, s.portionCount, s.unit, s.portionSize);
                        w.WriteLine(line);

                    }
                }


                return true;
            }
            else
            {

                return false;
            }

        }


        void UpdateLine(int nr, string editedline, string path)
        {

            string[] text = File.ReadAllLines(path);
            text[nr] = editedline;
            File.WriteAllLines(path, text);

        }

        void RemoveLine(int nr, string path)
        {

            string[] text = File.ReadAllLines(path);
            string[] textFinal = new string[text.Length-1];
            int counter = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if(i != nr+1)
                {
                    textFinal[counter] = text[i];
                    counter++;
                    
                }

            }

            File.WriteAllLines(path, textFinal);

        }


        void Help()
        {

            Console.WriteLine("How to use the program:");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Want to see current products stock?  Write   - \" STOCK \" ");
            Console.WriteLine("Want to see current restaurant menu? Write   - \" MENU \" ");
            Console.WriteLine("Want to see current customer orders? Write   - \" ORDERS \" ");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("To add new product / order / menu item write - \" ADD PRODUCT/ORDER/MENU \" ");
            Console.WriteLine("To update product / order / menu item write  - \" UPDATE PRODUCT/MENU \" ");
            Console.WriteLine("To delete product / order / menu item write  - \" REMOVE PRODUCT/MENU \" ");
            Console.WriteLine("");
            Console.WriteLine("If you need help later, write - HELP");
            Console.WriteLine("To exit the program write - EXIT");

        }

        static void Main(string[] args)
        {

            Program p = new Program();

            List<Stock> Stocks = new List<Stock>();
            List<Menu> Menus = new List<Menu>();
            List<Order> Orders = new List<Order>();



            p.Help();


            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.WriteLine("What do you want to do?");
                Console.Write(">>> ");
                string x = Console.ReadLine();
                Console.WriteLine();

                //------------------- Reading data -----------------

                if (string.Equals(x, "STOCK", StringComparison.CurrentCultureIgnoreCase))
                {
                    Stocks = p.ReadAllStock(true);
                }

                else if (string.Equals(x, "MENU", StringComparison.CurrentCultureIgnoreCase))
                {
                    Menus = p.ReadAllMenu(true);
                }

                else if (string.Equals(x, "ORDERS", StringComparison.CurrentCultureIgnoreCase))
                {
                    Orders = p.ReadAllOrders(true);
                }

                //---------------------------------------------------
                //------------------- Adding data -------------------

                else if (string.Equals(x, "ADD PRODUCT", StringComparison.CurrentCultureIgnoreCase))
                {

                    Console.WriteLine("Name of the product:");
                    Console.Write(">>> ");
                    string tmpName = Console.ReadLine();

                    Console.WriteLine("Product portion count:");
                    Console.Write(">>> ");
                    string tmpPortionCount = Console.ReadLine();

                    Console.WriteLine("Product unit:");
                    Console.Write(">>> ");
                    string tmpUnit = Console.ReadLine();

                    Console.WriteLine("Portion size of the product:");
                    Console.Write(">>> ");
                    string tmpPortionSize = Console.ReadLine();

                    int cnt = p.getLinesCount("../../../data/Stock.csv");

                    string line = string.Format("{0},{1},{2},{3},{4}", cnt.ToString(), tmpName, tmpPortionCount, tmpUnit, tmpPortionSize);
                    p.addLine(line, "../../../data/Stock.csv");

                }


                else if (string.Equals(x, "ADD MENU", StringComparison.CurrentCultureIgnoreCase))
                {

                    Console.WriteLine("Name of the meal:");
                    Console.Write(">>> ");
                    string tmpName = Console.ReadLine();

                    Console.WriteLine("Products used:");
                    Console.Write(">>> ");
                    string tmpProducts = Console.ReadLine();


                    int cnt = p.getLinesCount("../../../data/Menu.csv");

                    string line = string.Format("{0},{1},{2}", cnt.ToString(), tmpName, tmpProducts);
                    p.addLine(line, "../../../data/Menu.csv");

                }

                else if (string.Equals(x, "ADD ORDER", StringComparison.CurrentCultureIgnoreCase))
                {


                    Console.WriteLine("Meals ordered (separate by space):");
                    Console.Write(">>> ");
                    string tmpMeals = Console.ReadLine();

                    bool canBeAdded = p.deductFromStock(tmpMeals);

                    if (canBeAdded == true)
                    {
                        int cnt = p.getLinesCount("../../../data/Orders.csv");

                        string line = string.Format("{0},{1},{2}", cnt.ToString(), DateTime.Now, tmpMeals);
                        p.addLine(line, "../../../data/Orders.csv");

                    }
                    else
                    {
                        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine("The order cannot be added");
                    }


                }

                //-----------------------------------------------------
                //----------------- Updating data ---------------------

                else if (string.Equals(x, "UPDATE PRODUCT", StringComparison.CurrentCultureIgnoreCase))
                {
                    Stocks = p.ReadAllStock(false);
                    int counter = 0;

                    Console.WriteLine("ID of the item you want to update:");
                    Console.Write(">>> ");
                    string tmpId = Console.ReadLine();

                    foreach (Stock s in Stocks)
                    {
                        counter++;
                        if (s.id == tmpId)
                        {
                            Console.WriteLine("Change name of the product(leave blank to not change):");
                            Console.Write(">>> ");
                            string tmpName = Console.ReadLine();
                            if (tmpName == "")
                            {
                                tmpName = s.name;
                            }

                            Console.WriteLine("Change portion count of the product(leave blank to not change):");
                            Console.Write(">>> ");
                            string tmpPortionCnt = Console.ReadLine();
                            if (tmpPortionCnt == "")
                            {
                                tmpPortionCnt = s.portionCount.ToString();
                            }

                            Console.WriteLine("Change unit of the product(leave blank to not change):");
                            Console.Write(">>> ");
                            string tmpUnit = Console.ReadLine();
                            if (tmpUnit == "")
                            {
                                tmpUnit = s.unit;
                            }

                            Console.WriteLine("Change portion size of the product(leave blank to not change):");
                            Console.Write(">>> ");
                            string tmpPortionSz = Console.ReadLine();
                            if (tmpPortionSz == "")
                            {
                                tmpPortionSz = s.portionSize;
                            }

                            string changedLine = string.Format("{0},{1},{2},{3},{4}", tmpId, tmpName, tmpPortionCnt, tmpUnit, tmpPortionSz);
                            p.UpdateLine(counter, changedLine, "../../../data/Stock.csv");

                        }
                    }

                }

                else if (string.Equals(x, "UPDATE MENU", StringComparison.CurrentCultureIgnoreCase))
                {
                    Menus = p.ReadAllMenu(false);
                    int counter = 0;

                    Console.WriteLine("ID of the item you want to update:");
                    Console.Write(">>> ");
                    string tmpId = Console.ReadLine();

                    foreach (Menu s in Menus)
                    {
                        counter++;
                        if (s.id == tmpId)
                        {
                            Console.WriteLine("Change name of the meal(leave blank to not change):");
                            Console.Write(">>> ");
                            string tmpName = Console.ReadLine();
                            if (tmpName == "")
                            {
                                tmpName = s.name;
                            }

                            Console.WriteLine("Change products of the meal(leave blank to not change):");
                            Console.Write(">>> ");
                            string tmpProducts = Console.ReadLine();
                            if (tmpProducts == "")
                            {
                                tmpProducts = s.products;
                            }


                            string changedLine = string.Format("{0},{1},{2}", tmpId, tmpName, tmpProducts);
                            p.UpdateLine(counter, changedLine, "../../../data/Menu.csv");

                        }
                    }
                }

                //----------------- Removing data ---------------------
                //-----------------------------------------------------

                else if (string.Equals(x, "REMOVE PRODUCT", StringComparison.CurrentCultureIgnoreCase))
                {

                    Stocks = p.ReadAllStock(false);

                    Console.WriteLine("ID of the item you want to remove:");
                    Console.Write(">>> ");
                    string tmpId = Console.ReadLine();

                    int counter = 0;

                    foreach(Stock s in Stocks)
                    {
                        if(s.id == tmpId)
                        {
                            break;
                        }
                        counter++;
                    }


                    p.RemoveLine(counter, "../../../data/Stock.csv");

                }


                else if (string.Equals(x, "REMOVE MENU", StringComparison.CurrentCultureIgnoreCase))
                {

                    Menus = p.ReadAllMenu(false);

                    Console.WriteLine("ID of the item you want to remove:");
                    Console.Write(">>> ");
                    string tmpId = Console.ReadLine();

                    int counter = 0;

                    foreach (Menu s in Menus)
                    {
                        if (s.id == tmpId)
                        {
                            break;
                        }
                        counter++;
                    }


                    p.RemoveLine(counter, "../../../data/Menu.csv");

                }



                else if (string.Equals(x, "HELP", StringComparison.CurrentCultureIgnoreCase))
                {
                    p.Help();
                }

                else if (string.Equals(x, "EXIT", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }

            }



        }
    }
}
