// Data/DietData.cs
using HealthBite.Models;
using System.Collections.Generic;

namespace HealthBite.Data
{
    public static class DietData
    {
        public static List<DietModel> GetPredefinedDiets()
        {
            return new List<DietModel>
            {
                // ---- Kategori: Kilo Alma ----
                new DietModel {
                    Id = 1, Title = "Kas Yapıcı Güç Diyeti", Goal = "Kilo Alma", Description = "Protein ağırlıklı kas gelişimini destekleyen diyet.", Calories = 2500,
                    Meals = new Dictionary<string, MealPlan> {
                        { "Breakfast", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Yulaf Ezmesi", Portion = "1 kase (50g)", Calories = 190, Protein = 7, Carbs = 33, Fat = 4 },
                                new FoodDetailModel { Name = "Protein Tozu", Portion = "1 ölçek", Calories = 120, Protein = 24, Carbs = 3, Fat = 1 },
                                new FoodDetailModel { Name = "Muz", Portion = "1 adet orta", Calories = 105, Protein = 1, Carbs = 27, Fat = 0 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Tam Buğday Ekmeği", Portion = "2 dilim", Calories = 160, Protein = 8, Carbs = 30, Fat = 2 },
                                new FoodDetailModel { Name = "Yumurta (Haşlanmış)", Portion = "3 adet", Calories = 210, Protein = 18, Carbs = 2, Fat = 15 }
                            }
                        }},
                        { "Lunch", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Tavuk Göğsü (Izgara)", Portion = "200g", Calories = 330, Protein = 60, Carbs = 0, Fat = 7 },
                                new FoodDetailModel { Name = "Esmer Pirinç", Portion = "150g (pişmiş)", Calories = 170, Protein = 4, Carbs = 35, Fat = 2 },
                                new FoodDetailModel { Name = "Brokoli (Haşlanmış)", Portion = "1 kase", Calories = 55, Protein = 4, Carbs = 11, Fat = 1 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Kırmızı Mercimek Yemeği", Portion = "1 kase", Calories = 300, Protein = 20, Carbs = 45, Fat = 5 },
                                new FoodDetailModel { Name = "Bulgur Pilavı", Portion = "1 tabak", Calories = 200, Protein = 6, Carbs = 40, Fat = 2 }
                            }
                        }},
                        { "Dinner", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Somon (Fırında)", Portion = "150g", Calories = 310, Protein = 30, Carbs = 0, Fat = 20 },
                                new FoodDetailModel { Name = "Tatlı Patates (Fırında)", Portion = "1 adet orta", Calories = 100, Protein = 2, Carbs = 24, Fat = 0 },
                                new FoodDetailModel { Name = "Yeşil Salata (Zeytinyağlı)", Portion = "Büyük kase", Calories = 150, Protein = 3, Carbs = 10, Fat = 12 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Hindi Füme", Portion = "150g", Calories = 180, Protein = 30, Carbs = 2, Fat = 6 },
                                new FoodDetailModel { Name = "Kinoa Salatası", Portion = "1 kase", Calories = 220, Protein = 8, Carbs = 35, Fat = 5 }
                            }
                        }}
                    }
                },
                new DietModel {
                    Id = 2, Title = "Hacim Kazandıran Plan", Goal = "Kilo Alma", Description = "Yüksek kalori ve karbonhidrat içerikli plan.", Calories = 2800,
                    Meals = new Dictionary<string, MealPlan> {
                        { "Breakfast", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Tam Tahıllı Ekmek", Portion = "2 dilim", Calories = 180, Protein = 8, Carbs = 34, Fat = 2 },
                                new FoodDetailModel { Name = "Fıstık Ezmesi", Portion = "2 yemek kaşığı", Calories = 190, Protein = 8, Carbs = 7, Fat = 16 },
                                new FoodDetailModel { Name = "Tam Yağlı Süt", Portion = "1 bardak", Calories = 150, Protein = 8, Carbs = 12, Fat = 8 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Granola (Ballı)", Portion = "1 kase", Calories = 400, Protein = 10, Carbs = 60, Fat = 15 },
                                new FoodDetailModel { Name = "Meyveli Yoğurt", Portion = "1 büyük kase", Calories = 250, Protein = 12, Carbs = 40, Fat = 5 }
                            }
                        }},
                        { "Lunch", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Kırmızı Et (Biftek)", Portion = "200g", Calories = 450, Protein = 50, Carbs = 0, Fat = 28 },
                                new FoodDetailModel { Name = "Makarna (Tam Buğday)", Portion = "200g (pişmiş)", Calories = 300, Protein = 12, Carbs = 60, Fat = 2 },
                                new FoodDetailModel { Name = "Karışık Sebzeler (Sote)", Portion = "1 kase", Calories = 100, Protein = 3, Carbs = 15, Fat = 4 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Tavuk But (Fırında)", Portion = "2 adet", Calories = 400, Protein = 45, Carbs = 5, Fat = 20 },
                                new FoodDetailModel { Name = "Patates Püresi", Portion = "1 büyük tabak", Calories = 250, Protein = 5, Carbs = 45, Fat = 5 }
                            }
                        }},
                        { "Dinner", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Tavuk But (Haşlama)", Portion = "250g", Calories = 380, Protein = 45, Carbs = 0, Fat = 20 },
                                new FoodDetailModel { Name = "Pirinç Pilavı", Portion = "200g", Calories = 260, Protein = 5, Carbs = 58, Fat = 1 },
                                new FoodDetailModel { Name = "Yoğurt", Portion = "1 kase", Calories = 120, Protein = 10, Carbs = 15, Fat = 3 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Balık (Levrek Izgara)", Portion = "200g", Calories = 300, Protein = 40, Carbs = 0, Fat = 15 },
                                new FoodDetailModel { Name = "Nohutlu Pilav", Portion = "1 tabak", Calories = 350, Protein = 12, Carbs = 60, Fat = 8 }
                            }
                        }}
                    }
                },
                // ... Kilo Alma için 3 diyet daha eklenecek (Id: 3, 4, 5) - benzer şekilde doldurun ...
                // Örnek olarak Id=3 için iskelet:
                new DietModel {
                    Id = 3, Title = "Hardgainer Beslenme Programı", Goal = "Kilo Alma", Description = "Zor kilo alanlar için özel beslenme.", Calories = 3000,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Yulaf (bol)", Portion="1.5 kase", Calories=400, Protein=15, Carbs=70, Fat=8}}, AlternativeFoodItems = new List<FoodDetailModel>{new FoodDetailModel { Name="Muzlu Süt", Portion="2 bardak", Calories=300, Protein=10, Carbs=50, Fat=5}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Kuzu Pirzola", Portion="250g", Calories=600, Protein=50, Carbs=5, Fat=40}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Köfte (dana)", Portion="200g", Calories=450, Protein=40, Carbs=10, Fat=30}}}},
                    }
                },
                 new DietModel {
                    Id = 4, Title = "Vücut Geliştirici Öğünler", Goal = "Kilo Alma", Description = "Kas kazanımı için optimize edilmiş menü.", Calories = 2700,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Omlet (4 yumurta)", Portion="1 porsiyon", Calories=350, Protein=28, Carbs=3, Fat=24}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Ton Balığı (zeytinyağlı)", Portion="1 büyük konserve", Calories=300, Protein=40, Carbs=0, Fat=15}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Tavuk Sote (bol sebzeli)", Portion="200g tavuk", Calories=400, Protein=45, Carbs=15, Fat=18}}}},
                    }
                },
                new DietModel {
                    Id = 5, Title = "Besin Yoğunluğu Diyeti", Goal = "Kilo Alma", Description = "Yüksek enerji içerikli doğal besinler.", Calories = 2900,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Sütlaç (ev yapımı)", Portion="1 kase", Calories=300, Protein=8, Carbs=50, Fat=8}, new FoodDetailModel { Name="Ceviz & Badem", Portion="1 avuç", Calories=200, Protein=5, Carbs=7, Fat=18}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Kuzu İncik (haşlama)", Portion="200g", Calories=500, Protein=40, Carbs=5, Fat=35}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Fırında Tavuk (bütün but)", Portion="250g", Calories=450, Protein=50, Carbs=0, Fat=25}}}},
                    }
                },

                // ---- Kategori: Kilo Verme ----
                new DietModel {
                    Id = 6, Title = "Ketojenik Yağ Yakım Diyeti", Goal = "Kilo Verme", Description = "Düşük karbonhidrat, yüksek yağ esaslı diyet.", Calories = 1800,
                    Meals = new Dictionary<string, MealPlan> {
                        { "Breakfast", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Yumurtalı Omlet (2 yumurta, peynirli)", Portion = "1 porsiyon", Calories = 300, Protein = 20, Carbs = 3, Fat = 23 },
                                new FoodDetailModel { Name = "Avokado", Portion = "Yarım", Calories = 160, Protein = 2, Carbs = 9, Fat = 15 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Hindi Füme & Peynir Tabağı", Portion = "100g hindi, 50g peynir", Calories = 350, Protein = 30, Carbs = 2, Fat = 25 }
                            }
                        }},
                        { "Lunch", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Izgara Tavuk Salatası (bol yeşillik, zeytinyağlı)", Portion = "1 büyük kase", Calories = 450, Protein = 40, Carbs = 10, Fat = 28 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Ton Balıklı Salata (mayonezsiz)", Portion = "1 büyük kase", Calories = 350, Protein = 35, Carbs = 8, Fat = 20 }
                            }
                        }},
                        { "Dinner", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Somon Fırında (yanında kuşkonmaz)", Portion = "150g somon, 1 demet kuşkonmaz", Calories = 400, Protein = 35, Carbs = 8, Fat = 25 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Köri Soslu Tavuk (kremasız, yanında karnabahar püresi)", Portion = "150g tavuk", Calories = 380, Protein = 38, Carbs = 12, Fat = 20 }
                            }
                        }}
                    }
                },
                // ... Kilo Verme için 4 diyet daha eklenecek (Id: 7, 8, 9, 10) - benzer şekilde doldurun ...
                new DietModel {
                    Id = 7, Title = "Intermittent Fasting Planı", Goal = "Kilo Verme", Description = "Aralıklı oruç ile kalori kısıtlaması.", Calories = 1600,
                     Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ /* Genelde atlanır veya çok hafif */ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Sade Kahve/Çay", Portion="Limitsiz", Calories=5, Protein=0, Carbs=1, Fat=0}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Hindi Fümeli Bol Yeşillikli Salata", Portion="1 büyük kase", Calories=400, Protein=30, Carbs=15, Fat=20}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Haşlanmış Sebzeler ve Izgara Tavuk", Portion="150g tavuk", Calories=450, Protein=40, Carbs=20, Fat=20}}}},
                    }
                },
                new DietModel {
                    Id = 8, Title = "Detoks ve Temiz Beslenme", Goal = "Kilo Verme", Description = "Vücudu arındıran sebze ağırlıklı program.", Calories = 1500,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Yeşil Smoothie (ıspanak, salatalık, elma)", Portion="1 büyük bardak", Calories=200, Protein=5, Carbs=40, Fat=2}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Zeytinyağlı Enginar", Portion="2 adet", Calories=250, Protein=6, Carbs=25, Fat=15}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Mercimek Çorbası (bol limonlu)", Portion="1 büyük kase", Calories=300, Protein=15, Carbs=45, Fat=5}}}},
                    }
                },
                new DietModel {
                    Id = 9, Title = "Glisemik Kontrol Diyeti", Goal = "Kilo Verme", Description = "Kan şekeri dengesi odaklı menü.", Calories = 1700,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Yulaf Lapası (tarçınlı, meyvesiz)", Portion="1 kase", Calories=200, Protein=7, Carbs=35, Fat=4}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Nohut Yemeği (etli veya etsiz)", Portion="1 tabak", Calories=400, Protein=18, Carbs=50, Fat=12}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Izgara Sebzeler ve Balık", Portion="150g balık", Calories=450, Protein=35, Carbs=15, Fat=25}}}},
                    }
                },
                new DietModel {
                    Id = 10, Title = "FitForm Yağ Yakıcı Plan", Goal = "Kilo Verme", Description = "Metabolizmayı hızlandıran haftalık plan.", Calories = 1750,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Haşlanmış Yumurta", Portion="2 adet", Calories=150, Protein=12, Carbs=1, Fat=10}, new FoodDetailModel { Name="Salatalık & Domates", Portion="Limitsiz", Calories=50, Protein=2, Carbs=10, Fat=0}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Tavuklu Yeşil Salata (nar ekşili)", Portion="1 büyük kase", Calories=400, Protein=38, Carbs=15, Fat=20}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Fırında Sebzeli Balık Buğulama", Portion="200g balık", Calories=350, Protein=40, Carbs=10, Fat=18}}}},
                    }
                },

                // ---- Kategori: Kilo Koruma ----
                new DietModel {
                    Id = 11, Title = "Akdeniz Kilo Koruma Diyeti", Goal = "Kilo Koruma", Description = "Zeytinyağı ve sebze ağırlıklı dengeli beslenme.", Calories = 2000,
                    Meals = new Dictionary<string, MealPlan> {
                        { "Breakfast", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Tam Buğday Ekmeği", Portion = "1 dilim", Calories = 80, Protein = 4, Carbs = 15, Fat = 1 },
                                new FoodDetailModel { Name = "Beyaz Peynir", Portion = "50g", Calories = 130, Protein = 7, Carbs = 1, Fat = 11 },
                                new FoodDetailModel { Name = "Domates, Salatalık, Zeytin", Portion = "Bolca", Calories = 100, Protein = 2, Carbs = 10, Fat = 7 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Menemen (2 yumurtalı)", Portion = "1 porsiyon", Calories = 250, Protein = 15, Carbs = 10, Fat = 18 }
                            }
                        }},
                        { "Lunch", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Zeytinyağlı Enginar", Portion = "1 adet büyük", Calories = 200, Protein = 5, Carbs = 20, Fat = 12 },
                                new FoodDetailModel { Name = "Tam Tahıllı Makarna Salatası", Portion = "1 kase", Calories = 350, Protein = 12, Carbs = 50, Fat = 10 },
                                new FoodDetailModel { Name = "Yoğurt", Portion = "1 kase", Calories = 120, Protein = 10, Carbs = 15, Fat = 3 }
                            },
                            AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Izgara Köfte ve Piyaz", Portion = "150g köfte, 1 kase piyaz", Calories = 450, Protein = 30, Carbs = 25, Fat = 25 }
                            }
                        }},
                        { "Dinner", new MealPlan {
                            PrimaryFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Izgara Balık (Levrek/Çipura)", Portion = "150-200g", Calories = 300, Protein = 40, Carbs = 0, Fat = 15 },
                                new FoodDetailModel { Name = "Roka Salatası (Narlı)", Portion = "Büyük kase", Calories = 150, Protein = 3, Carbs = 15, Fat = 9 },
                                new FoodDetailModel { Name = "Meyve", Portion = "1 porsiyon", Calories = 80, Protein = 1, Carbs = 20, Fat = 0 }
                            },
                             AlternativeFoodItems = new List<FoodDetailModel> {
                                new FoodDetailModel { Name = "Sebzeli Tavuk Sote", Portion = "1 tabak", Calories = 350, Protein = 35, Carbs = 20, Fat = 15 }
                            }
                        }}
                    }
                },
                // ... Kilo Koruma için 4 diyet daha eklenecek (Id: 12, 13, 14, 15) - benzer şekilde doldurun ...
                new DietModel {
                    Id = 12, Title = "Ofis İçin Pratik Koruma Planı", Goal = "Kilo Koruma", Description = "Yoğun tempoda kolayca uygulanabilen öğünler.", Calories = 1900,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Peynirli Tam Buğday Sandviç", Portion="1 adet", Calories=350, Protein=20, Carbs=40, Fat=12}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Ton Balıklı Salata (hazır paket veya ev yapımı)", Portion="1 büyük kase", Calories=400, Protein=30, Carbs=20, Fat=20}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Sebzeli Omlet (2 yumurta)", Portion="1 porsiyon", Calories=300, Protein=18, Carbs=10, Fat=22}, new FoodDetailModel { Name="Yoğurt (yarım yağlı)", Portion="1 kase", Calories=100, Protein=8, Carbs=12, Fat=2}}}},
                    }
                },
                new DietModel {
                    Id = 13, Title = "Haftalık Dengeli Beslenme", Goal = "Kilo Koruma", Description = "Her gün farklı ve dengeli besinler içeren menü.", Calories = 2100,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Yulaf Ezmesi (meyveli, cevizli)", Portion="1 kase", Calories=380, Protein=12, Carbs=60, Fat=10}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Sebzeli Tavuk Sote & Bulgur Pilavı", Portion="1 tabak", Calories=500, Protein=40, Carbs=55, Fat=15}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Zeytinyağlı Taze Fasulye & Cacık", Portion="1 tabak fasulye, 1 kase cacık", Calories=400, Protein=10, Carbs=40, Fat=22}}}},
                    }
                },
                new DietModel {
                    Id = 14, Title = "Sporcu Koruma Menüsü", Goal = "Kilo Koruma", Description = "Aktif kişiler için dengeli protein-karbonhidrat.", Calories = 2300,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Yumurtalı Kahvaltı Tabağı (3 yumurta, bol yeşillik, peynir)", Portion="1 tabak", Calories=450, Protein=30, Carbs=15, Fat=30}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Tavuklu Kinoa Salatası (bol malzemeli)", Portion="1 büyük kase", Calories=550, Protein=45, Carbs=50, Fat=20}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Fırında Somon ve Brokoli", Portion="200g somon, bol brokoli", Calories=500, Protein=45, Carbs=15, Fat=30}}}},
                    }
                },
                new DietModel {
                    Id = 15, Title = "Günlük Kalori Dengesi Diyeti", Goal = "Kilo Koruma", Description = "Her öğünde eşit makro dağılımı.", Calories = 2050,
                    Meals = new Dictionary<string, MealPlan> {
                        {"Breakfast", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Tam Tahıllı Tost (peynirli, domatesli)", Portion="2 dilim ekmek", Calories=350, Protein=18, Carbs=35, Fat=15}, new FoodDetailModel { Name="Süt (laktozsuz)", Portion="1 bardak", Calories=120, Protein=8, Carbs=12, Fat=5}}}},
                        {"Lunch", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Haşlanmış Sebzeli Izgara Tavuk Göğsü", Portion="150g tavuk", Calories=450, Protein=40, Carbs=30, Fat=18}}}},
                        {"Dinner", new MealPlan{ PrimaryFoodItems = new List<FoodDetailModel>{ new FoodDetailModel { Name="Mevsim Sebzeleri Çorbası (kremasız)", Portion="1 büyük kase", Calories=250, Protein=8, Carbs=35, Fat=8}, new FoodDetailModel { Name="Kuru Fasulye (etsiz)", Portion="Yarım tabak", Calories=300, Protein=15, Carbs=50, Fat=5}}}},
                    }
                },
            };
        }
    }
}