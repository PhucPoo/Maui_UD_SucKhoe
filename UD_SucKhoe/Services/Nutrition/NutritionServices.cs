using UD_SucKhoe.Models;
namespace UD_SucKhoe.Services.Nutrition;

public class NutritionService : INutritionService
{
    public MealPlan GetMealsByBMI(double bmi)
    {
        var meal = new MealPlan();
        if (bmi < 18.5)
        {
            // Tăng cân
            meal.Breakfast.AddRange(new[]
            {
                "Bánh mì + trứng ốp la",
                "Sinh tố chuối + sữa chua",
                "Yến mạch + sữa tươi"
            });
            meal.Lunch.AddRange(new[]
            {
                "Cơm + thịt gà + rau xanh",
                "Canh xương",
                "Tráng miệng chuối"
            });
            meal.Snack.AddRange(new[]
            {
                "Sữa giàu năng lượng",
                "Hạnh nhân, óc chó"
            });
            meal.Dinner.AddRange(new[]
            {
                "Cơm + cá hồi + rau củ",
                "Khoai lang",
                "Sữa nóng"
            });
        }
        else if (bmi < 24.9)
        {
            // Duy trì
            meal.Breakfast.AddRange(new[]
            {
                "Bánh mì gạo lứt + trứng",
                "Trà xanh"
            });
            meal.Lunch.AddRange(new[]
            {
                "Cơm + thịt nạc + rau",
                "Canh bí"
            });
            meal.Snack.AddRange(new[]
            {
                "Sữa chua không đường",
                "Táo"
            });
            meal.Dinner.AddRange(new[]
            {
                "Salad ức gà",
                "Khoai tây nghiền"
            });
        }
        return meal;
    }

    public Dictionary<string, MealPlan> GetWeeklyMealsByBMI(double bmi)
    {
        var weeklyPlan = new Dictionary<string, MealPlan>();

        if (bmi < 18.5)
        {
            // THỰC ĐƠN TĂNG CÂN - Giàu năng lượng, protein

            // Thứ 2
            weeklyPlan["Thứ 2"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Phở bò đặc biệt",
                    "Trứng gà luộc (2 quả)",
                    "Sữa đậu nành + chuối"
                },
                Lunch = new List<string>
                {
                    "Cơm trắng (2 bát)",
                    "Thịt ba chỉ rim",
                    "Đậu hũ chiên",
                    "Canh cải thịt",
                    "Chuối"
                },
                Snack = new List<string>
                {
                    "Bánh bông lan",
                    "Sữa tươi có đường",
                    "Hạnh nhân (1 nắm)"
                },
                Dinner = new List<string>
                {
                    "Cơm trắng",
                    "Cá hồi nướng bơ",
                    "Rau củ xào",
                    "Khoai lang luộc",
                    "Sữa nóng"
                }
            };

            // Thứ 3
            weeklyPlan["Thứ 3"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bánh mì pate + thịt nguội",
                    "Trứng ốp la (2 quả)",
                    "Sinh tố bơ + sữa"
                },
                Lunch = new List<string>
                {
                    "Cơm chiên dương châu",
                    "Sườn nướng",
                    "Canh thịt nấu rau củ",
                    "Dưa hấu"
                },
                Snack = new List<string>
                {
                    "Sữa chua Hy Lạp",
                    "Granola",
                    "Óc chó"
                },
                Dinner = new List<string>
                {
                    "Mì Ý sốt kem",
                    "Thịt gà nướng",
                    "Salad rau trộn dầu ô liu",
                    "Nước ép cam"
                }
            };

            // Thứ 4
            weeklyPlan["Thứ 4"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Xôi gà",
                    "Trứng vịt muối",
                    "Sữa đậu nành"
                },
                Lunch = new List<string>
                {
                    "Cơm trắng (2 bát)",
                    "Bò kho",
                    "Đậu phụ sốt cà",
                    "Canh rau ngót",
                    "Xoài"
                },
                Snack = new List<string>
                {
                    "Bánh quy bơ",
                    "Sữa tươi",
                    "Chuối"
                },
                Dinner = new List<string>
                {
                    "Cơm trắng",
                    "Cá thu kho",
                    "Trứng chiên",
                    "Rau luộc",
                    "Sữa nóng"
                }
            };

            // Thứ 5
            weeklyPlan["Thứ 5"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bánh cuốn nhân thịt",
                    "Chả lụa",
                    "Nước cam ép"
                },
                Lunch = new List<string>
                {
                    "Cơm gà Hải Nam",
                    "Gà luộc",
                    "Súp gà",
                    "Dưa chuột",
                    "Nho"
                },
                Snack = new List<string>
                {
                    "Smoothie chuối + yến mạch",
                    "Hạt điều"
                },
                Dinner = new List<string>
                {
                    "Cơm trắng",
                    "Thịt heo nấu đông",
                    "Canh chua cá",
                    "Khoai lang tím",
                    "Sữa"
                }
            };

            // Thứ 6
            weeklyPlan["Thứ 6"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bún bò Huế",
                    "Giò heo",
                    "Trứng gà luộc"
                },
                Lunch = new List<string>
                {
                    "Cơm trắng (2 bát)",
                    "Gà rán",
                    "Đậu que xào thịt",
                    "Canh bí đỏ",
                    "Ổi"
                },
                Snack = new List<string>
                {
                    "Bánh flan",
                    "Sữa tươi",
                    "Hạt macca"
                },
                Dinner = new List<string>
                {
                    "Cơm trắng",
                    "Cá diêu hồng kho tộ",
                    "Đậu hũ sốt cà chua",
                    "Rau muống xào",
                    "Sữa nóng"
                }
            };

            // Thứ 7
            weeklyPlan["Thứ 7"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bánh mì nướng bơ",
                    "Pate + trứng",
                    "Cacao nóng + sữa"
                },
                Lunch = new List<string>
                {
                    "Cơm trắng",
                    "Sườn xào chua ngọt",
                    "Tôm rang",
                    "Canh chua",
                    "Sapoche"
                },
                Snack = new List<string>
                {
                    "Bánh bao nhân thịt",
                    "Sữa đậu nành"
                },
                Dinner = new List<string>
                {
                    "Lẩu gà lá é",
                    "Cơm trắng",
                    "Bún tươi",
                    "Sữa tươi"
                }
            };

            // Chủ nhật
            weeklyPlan["Chủ nhật"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bánh bao chiên",
                    "Xíu mại",
                    "Sữa đậu nành + đường"
                },
                Lunch = new List<string>
                {
                    "Bún chả Hà Nội",
                    "Nem rán",
                    "Rau sống",
                    "Chè đậu xanh"
                },
                Snack = new List<string>
                {
                    "Sinh tố bơ dừa",
                    "Bánh quy socola"
                },
                Dinner = new List<string>
                {
                    "Cơm trắng",
                    "Gà kho gừng",
                    "Trứng hấp",
                    "Rau cải luộc",
                    "Sữa chua có đường"
                }
            };
        }
        else if (bmi >= 18.5 && bmi <= 24.9)
        {
            // THỰC ĐƠN DUY TRÌ - Cân bằng dinh dưỡng

            // Thứ 2
            weeklyPlan["Thứ 2"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bánh mì nguyên cám + trứng",
                    "Sữa tươi không đường",
                    "Táo"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt (1 bát)",
                    "Ức gà luộc",
                    "Đậu hũ hấp",
                    "Rau xào",
                    "Canh rau"
                },
                Snack = new List<string>
                {
                    "Sữa chua không đường",
                    "Hạt hạnh nhân (5-7 hạt)"
                },
                Dinner = new List<string>
                {
                    "Cơm gạo lứt",
                    "Cá hồi nướng",
                    "Rau luộc",
                    "Canh bí đao"
                }
            };

            // Thứ 3
            weeklyPlan["Thứ 3"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Yến mạch + sữa tươi",
                    "Chuối",
                    "Trà xanh"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt",
                    "Thịt bò xào",
                    "Đậu que luộc",
                    "Canh chua chay",
                    "Cam"
                },
                Snack = new List<string>
                {
                    "Nước ép cà rốt",
                    "Bánh quy yến mạch"
                },
                Dinner = new List<string>
                {
                    "Salad ức gà",
                    "Khoai lang luộc",
                    "Trứng luộc",
                    "Súp rau củ"
                }
            };

            // Thứ 4
            weeklyPlan["Thứ 4"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bánh mì sandwich rau củ",
                    "Trứng luộc",
                    "Sữa đậu nành không đường"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt",
                    "Cá thu nướng",
                    "Đậu hũ sốt cà",
                    "Rau muống luộc",
                    "Dưa hấu"
                },
                Snack = new List<string>
                {
                    "Sữa chua",
                    "Táo"
                },
                Dinner = new List<string>
                {
                    "Cơm gạo lứt",
                    "Gà luộc",
                    "Canh rau củ",
                    "Trà xanh"
                }
            };

            // Thứ 5
            weeklyPlan["Thứ 5"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Phở gà không mỡ",
                    "Rau thơm",
                    "Chanh ớt"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt",
                    "Thịt nạc kho",
                    "Canh bí đỏ",
                    "Rau luộc",
                    "Ổi"
                },
                Snack = new List<string>
                {
                    "Nước ép dưa chuột",
                    "Hạt óc chó (3-5 hạt)"
                },
                Dinner = new List<string>
                {
                    "Cơm gạo lứt",
                    "Tôm hấp",
                    "Rau trộn",
                    "Canh cải"
                }
            };

            // Thứ 6
            weeklyPlan["Thứ 6"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Cháo yến mạch",
                    "Trứng luộc",
                    "Cà chua bi"
                },
                Lunch = new List<string>
                {
                    "Bún cá",
                    "Chả cá",
                    "Rau sống",
                    "Nước mắm gừng",
                    "Chuối"
                },
                Snack = new List<string>
                {
                    "Sữa chua Hy Lạp",
                    "Dâu tây"
                },
                Dinner = new List<string>
                {
                    "Cơm gạo lứt",
                    "Cá rô phi nướng",
                    "Rau xào nấm",
                    "Canh rau"
                }
            };

            // Thứ 7
            weeklyPlan["Thứ 7"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bánh mì nguyên cám",
                    "Bơ đậu phộng",
                    "Nước ép táo"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt",
                    "Thịt bò hầm",
                    "Rau cải luộc",
                    "Canh nấm",
                    "Nho"
                },
                Snack = new List<string>
                {
                    "Smoothie xanh",
                    "Hạt chia"
                },
                Dinner = new List<string>
                {
                    "Salad cá ngừ",
                    "Khoai tây nghiền",
                    "Súp bí đỏ"
                }
            };

            // Chủ nhật
            weeklyPlan["Chủ nhật"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Bún chay",
                    "Đậu hũ chiên",
                    "Rau sống",
                    "Nước chanh"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt",
                    "Gà hấp nấm",
                    "Rau củ luộc",
                    "Canh miso",
                    "Xoài"
                },
                Snack = new List<string>
                {
                    "Sữa chua",
                    "Granola"
                },
                Dinner = new List<string>
                {
                    "Cơm gạo lứt",
                    "Cá kho tộ",
                    "Rau muống xào",
                    "Trà gạo lứt"
                }
            };
        }
        else // bmi > 24.9
        {
            // THỰC ĐƠN GIẢM CÂN - Ít calo, nhiều chất xơ

            // Thứ 2
            weeklyPlan["Thứ 2"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Yến mạch + sữa hạt đề",
                    "Trứng trắng luộc (2 quả)",
                    "Dưa chuột"
                },
                Lunch = new List<string>
                {
                    "Salad ức gà",
                    "Rau xà lách, cà chua",
                    "Dầu ô liu",
                    "Canh rau củ"
                },
                Snack = new List<string>
                {
                    "Táo xanh",
                    "Trà xanh không đường"
                },
                Dinner = new List<string>
                {
                    "Cá hấp",
                    "Rau luộc",
                    "Canh bí đao",
                    "Nước chanh"
                }
            };

            // Thứ 3
            weeklyPlan["Thứ 3"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Cháo yến mạch không đường",
                    "Cà chua bi",
                    "Trà gừng"
                },
                Lunch = new List<string>
                {
                    "Ức gà nướng",
                    "Rau củ hấp",
                    "Súp bí đỏ không dầu",
                    "Nước lọc"
                },
                Snack = new List<string>
                {
                    "Ổi",
                    "Trà ô long"
                },
                Dinner = new List<string>
                {
                    "Cá diêu hồng nướng",
                    "Salad rau trộn",
                    "Canh cải không dầu"
                }
            };

            // Thứ 4
            weeklyPlan["Thứ 4"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Trứng trắng luộc (3 quả)",
                    "Cà chua",
                    "Trà xanh"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt (0.5 bát)",
                    "Gà luộc bỏ da",
                    "Rau luộc",
                    "Canh rau không dầu"
                },
                Snack = new List<string>
                {
                    "Dưa hấu",
                    "Nước detox chanh bạc hà"
                },
                Dinner = new List<string>
                {
                    "Tôm hấp",
                    "Rau cải luộc",
                    "Súp nấm",
                    "Trà gạo lứt"
                }
            };

            // Thứ 5
            weeklyPlan["Thứ 5"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Smoothie rau xanh",
                    "Hạt chia",
                    "Trứng luộc"
                },
                Lunch = new List<string>
                {
                    "Bún chay",
                    "Rau sống",
                    "Đậu hũ luộc",
                    "Nước mắm chanh"
                },
                Snack = new List<string>
                {
                    "Cà rốt thái막대",
                    "Trà atiso"
                },
                Dinner = new List<string>
                {
                    "Cá thu nướng",
                    "Rau củ hấp",
                    "Canh rau ngót"
                }
            };

            // Thứ 6
            weeklyPlan["Thứ 6"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Yến mạch + sữa hạnh nhân",
                    "Quả việt quất",
                    "Trà xanh matcha"
                },
                Lunch = new List<string>
                {
                    "Ức gà xào rau củ",
                    "Súp bông cải xanh",
                    "Nước chanh ấm"
                },
                Snack = new List<string>
                {
                    "Cần tây",
                    "Nước ép cà chua"
                },
                Dinner = new List<string>
                {
                    "Salad tôm",
                    "Rau trộn",
                    "Canh rong biển"
                }
            };

            // Thứ 7
            weeklyPlan["Thứ 7"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Trứng trắng chiên không dầu",
                    "Dưa chuột",
                    "Cà chua bi",
                    "Trà gừng"
                },
                Lunch = new List<string>
                {
                    "Cơm gạo lứt (0.5 bát)",
                    "Cá rô phi hấp",
                    "Rau luộc",
                    "Canh chua chay"
                },
                Snack = new List<string>
                {
                    "Đu đủ xanh",
                    "Nước dừa tươi"
                },
                Dinner = new List<string>
                {
                    "Gà luộc bỏ da",
                    "Salad rau củ",
                    "Súp rau"
                }
            };

            // Chủ nhật
            weeklyPlan["Chủ nhật"] = new MealPlan
            {
                Breakfast = new List<string>
                {
                    "Cháo yến mạch + hạt chia",
                    "Trứng luộc",
                    "Trà ô long"
                },
                Lunch = new List<string>
                {
                    "Bún cá chay",
                    "Rau sống",
                    "Đậu hũ non",
                    "Mắm gừng"
                },
                Snack = new List<string>
                {
                    "Cam",
                    "Trà xanh"
                },
                Dinner = new List<string>
                {
                    "Ức gà nướng",
                    "Rau củ hấp",
                    "Canh khổ qua",
                    "Nước chanh không đường"
                }
            };
        }

        return weeklyPlan;
    }

    // Lấy thực đơn theo ngày trong tuần
    public MealPlan GetMealByDayOfWeek(double bmi, string dayOfWeek)
    {
        var weeklyPlan = GetWeeklyMealsByBMI(bmi);
        return weeklyPlan.ContainsKey(dayOfWeek) ? weeklyPlan[dayOfWeek] : new MealPlan();
    }

    // Chuyển đổi DayOfWeek sang tiếng Việt
    public string ConvertDayOfWeekToVietnamese(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Monday => "Thứ 2",
            DayOfWeek.Tuesday => "Thứ 3",
            DayOfWeek.Wednesday => "Thứ 4",
            DayOfWeek.Thursday => "Thứ 5",
            DayOfWeek.Friday => "Thứ 6",
            DayOfWeek.Saturday => "Thứ 7",
            DayOfWeek.Sunday => "Chủ nhật",
            _ => "Thứ 2"
        };
    }
}