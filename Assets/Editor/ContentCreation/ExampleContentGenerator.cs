using UnityEditor;
using UnityEngine;
using Runtime.Data.Enums;
using Runtime.Data.ScriptableObjects;
using Runtime.Data.Structs;

namespace Editor.ContentCreation
{
    /// <summary>
    /// Generates example content for testing the data foundation.
    /// Run from menu: Prompt Harvest > Generate Example Content
    /// </summary>
    public static class ExampleContentGenerator
    {
        [MenuItem("Prompt Harvest/Generate Example Content")]
        public static void GenerateAll()
        {
            GenerateFriendshipTiers();
            GenerateItems();
            GenerateCraftingStations();
            GenerateRecipes();
            GenerateCrops();
            GenerateMobs();
            GenerateBuildings();
            GenerateNPCs();
            GenerateQuests();
            GenerateActivities();
            GenerateEntities();
            GenerateDialogueOptions();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Example content generation complete!");
        }

        private static void GenerateFriendshipTiers()
        {
            // Stranger - base tier
            var stranger = ScriptableObject.CreateInstance<FriendshipTierData>();
            stranger.Id = "tier_stranger";
            stranger.Name = "Stranger";
            stranger.PointsRequired = 0;
            stranger.Unlocks = new TierUnlock[] { };
            stranger.Tags = new[] { "base" };
            SaveAsset(stranger, "Assets/Data/FriendshipTiers/Stranger.asset");

            // Acquaintance - 20 points
            var acquaintance = ScriptableObject.CreateInstance<FriendshipTierData>();
            acquaintance.Id = "tier_acquaintance";
            acquaintance.Name = "Acquaintance";
            acquaintance.PointsRequired = 20;
            acquaintance.Unlocks = new[] { TierUnlock.PriceModifier(0.95f) };
            acquaintance.Tags = new[] { "base" };
            SaveAsset(acquaintance, "Assets/Data/FriendshipTiers/Acquaintance.asset");

            // Friend - 40 points
            var friend = ScriptableObject.CreateInstance<FriendshipTierData>();
            friend.Id = "tier_friend";
            friend.Name = "Friend";
            friend.PointsRequired = 40;
            friend.Unlocks = new[] { TierUnlock.CanEnterHome(), TierUnlock.PriceModifier(0.9f) };
            friend.Tags = new[] { "base" };
            SaveAsset(friend, "Assets/Data/FriendshipTiers/Friend.asset");

            // Good Friend - 60 points
            var goodFriend = ScriptableObject.CreateInstance<FriendshipTierData>();
            goodFriend.Id = "tier_good_friend";
            goodFriend.Name = "Good Friend";
            goodFriend.PointsRequired = 60;
            goodFriend.Unlocks = new[] { TierUnlock.CanFollow(), TierUnlock.PriceModifier(0.85f) };
            goodFriend.Tags = new[] { "base" };
            SaveAsset(goodFriend, "Assets/Data/FriendshipTiers/GoodFriend.asset");

            // Best Friend - 80 points
            var bestFriend = ScriptableObject.CreateInstance<FriendshipTierData>();
            bestFriend.Id = "tier_best_friend";
            bestFriend.Name = "Best Friend";
            bestFriend.PointsRequired = 80;
            bestFriend.Unlocks = new[] { TierUnlock.PriceModifier(0.8f) };
            bestFriend.Tags = new[] { "base" };
            SaveAsset(bestFriend, "Assets/Data/FriendshipTiers/BestFriend.asset");

            Debug.Log("Generated 5 friendship tiers");
        }

        private static void GenerateItems()
        {
            // Basic Hoe - Tool
            var basicHoe = ScriptableObject.CreateInstance<ItemData>();
            basicHoe.Id = "item_basic_hoe";
            basicHoe.Name = "Basic Hoe";
            basicHoe.Type = ItemType.Tool;
            basicHoe.Description = "A simple hoe for tilling soil.";
            basicHoe.Value = 50;
            basicHoe.StackMax = 1;
            basicHoe.Durability = 100;
            basicHoe.Tier = "basic";
            basicHoe.Placeable = false;
            basicHoe.SpriteHint = "tool_hoe_basic";
            basicHoe.Tags = new[] { "tool", "farming", "starter" };
            SaveAsset(basicHoe, "Assets/Data/Items/BasicHoe.asset");

            // Iron Ore - Material
            var ironOre = ScriptableObject.CreateInstance<ItemData>();
            ironOre.Id = "item_iron_ore";
            ironOre.Name = "Iron Ore";
            ironOre.Type = ItemType.Material;
            ironOre.Description = "Raw iron ore from the mines. Can be smelted into bars.";
            ironOre.Value = 15;
            ironOre.StackMax = 99;
            ironOre.Durability = 0;
            ironOre.Placeable = false;
            ironOre.SpriteHint = "ore_iron";
            ironOre.Tags = new[] { "material", "ore", "metal" };
            SaveAsset(ironOre, "Assets/Data/Items/IronOre.asset");

            // Iron Bar - Material
            var ironBar = ScriptableObject.CreateInstance<ItemData>();
            ironBar.Id = "item_iron_bar";
            ironBar.Name = "Iron Bar";
            ironBar.Type = ItemType.Material;
            ironBar.Description = "A bar of refined iron. Used for crafting tools and equipment.";
            ironBar.Value = 40;
            ironBar.StackMax = 99;
            ironBar.Durability = 0;
            ironBar.Placeable = false;
            ironBar.SpriteHint = "bar_iron";
            ironBar.Tags = new[] { "material", "bar", "metal", "refined" };
            SaveAsset(ironBar, "Assets/Data/Items/IronBar.asset");

            // Turnip Seeds - Seed
            var turnipSeeds = ScriptableObject.CreateInstance<ItemData>();
            turnipSeeds.Id = "item_turnip_seeds";
            turnipSeeds.Name = "Turnip Seeds";
            turnipSeeds.Type = ItemType.Seed;
            turnipSeeds.Description = "Plant these in tilled soil to grow turnips.";
            turnipSeeds.Value = 10;
            turnipSeeds.StackMax = 99;
            turnipSeeds.Durability = 0;
            turnipSeeds.Placeable = true;
            turnipSeeds.PlaceableZones = new[] { Zone.Farm };
            turnipSeeds.SpriteHint = "seed_turnip";
            turnipSeeds.Tags = new[] { "seed", "crop", "spring" };
            SaveAsset(turnipSeeds, "Assets/Data/Items/TurnipSeeds.asset");

            // Turnip - Product
            var turnip = ScriptableObject.CreateInstance<ItemData>();
            turnip.Id = "item_turnip";
            turnip.Name = "Turnip";
            turnip.Type = ItemType.Product;
            turnip.Description = "A fresh turnip. Can be sold or used in cooking.";
            turnip.Value = 25;
            turnip.StackMax = 99;
            turnip.Durability = 0;
            turnip.DecayDays = 7;
            turnip.Placeable = false;
            turnip.SpriteHint = "crop_turnip";
            turnip.Tags = new[] { "product", "crop", "vegetable", "food" };
            SaveAsset(turnip, "Assets/Data/Items/Turnip.asset");

            // Egg - Product
            var egg = ScriptableObject.CreateInstance<ItemData>();
            egg.Id = "item_egg";
            egg.Name = "Egg";
            egg.Type = ItemType.Product;
            egg.Description = "A fresh chicken egg.";
            egg.Value = 20;
            egg.StackMax = 99;
            egg.Durability = 0;
            egg.DecayDays = 5;
            egg.Placeable = false;
            egg.SpriteHint = "product_egg";
            egg.Tags = new[] { "product", "animal", "food" };
            SaveAsset(egg, "Assets/Data/Items/Egg.asset");

            // Hay - Material (for horses)
            var hay = ScriptableObject.CreateInstance<ItemData>();
            hay.Id = "item_hay";
            hay.Name = "Hay";
            hay.Type = ItemType.Material;
            hay.Description = "Dried grass. Used to feed animals.";
            hay.Value = 5;
            hay.StackMax = 99;
            hay.Durability = 0;
            hay.Placeable = false;
            hay.SpriteHint = "material_hay";
            hay.Tags = new[] { "material", "feed", "animal" };
            SaveAsset(hay, "Assets/Data/Items/Hay.asset");

            Debug.Log("Generated 7 items");
        }

        private static void GenerateCraftingStations()
        {
            // Furnace
            var furnace = ScriptableObject.CreateInstance<CraftingStationData>();
            furnace.Id = "station_furnace";
            furnace.Name = "Furnace";
            furnace.StationType = StationType.Furnace;
            furnace.RecipesEnabled = new[] { "recipe_smelt_iron_bar" };
            furnace.PlaceableZones = new[] { Zone.Farm };
            furnace.SizeTiles = new TileSize(1, 1);
            furnace.CraftCost = new[]
            {
                new RecipeIngredient("item_stone", 20),
                new RecipeIngredient("item_clay", 10)
            };
            furnace.SpriteHint = "station_furnace";
            furnace.Tags = new[] { "station", "smelting", "base" };
            SaveAsset(furnace, "Assets/Data/CraftingStations/Furnace.asset");

            // Workbench
            var workbench = ScriptableObject.CreateInstance<CraftingStationData>();
            workbench.Id = "station_workbench";
            workbench.Name = "Workbench";
            workbench.StationType = StationType.Workbench;
            workbench.RecipesEnabled = new string[] { };
            workbench.PlaceableZones = new[] { Zone.Farm };
            workbench.SizeTiles = new TileSize(2, 1);
            workbench.CraftCost = new[]
            {
                new RecipeIngredient("item_wood", 30)
            };
            workbench.SpriteHint = "station_workbench";
            workbench.Tags = new[] { "station", "crafting", "base" };
            SaveAsset(workbench, "Assets/Data/CraftingStations/Workbench.asset");

            Debug.Log("Generated 2 crafting stations");
        }

        private static void GenerateRecipes()
        {
            // Smelt Iron Bar
            var smeltIron = ScriptableObject.CreateInstance<RecipeData>();
            smeltIron.Id = "recipe_smelt_iron_bar";
            smeltIron.Name = "Smelt Iron Bar";
            smeltIron.RecipeType = RecipeType.Smelting;
            smeltIron.Inputs = new[] { new RecipeIngredient("item_iron_ore", 5) };
            smeltIron.Output = new RecipeOutput("item_iron_bar", 1);
            smeltIron.StationRequiredId = "station_furnace";
            smeltIron.TimeHours = 1f;
            smeltIron.AdvancesTimePhase = false;
            smeltIron.Tags = new[] { "smelting", "metal", "base" };
            SaveAsset(smeltIron, "Assets/Data/Recipes/SmeltIronBar.asset");

            Debug.Log("Generated 1 recipe");
        }

        private static void GenerateCrops()
        {
            // Turnip
            var turnipCrop = ScriptableObject.CreateInstance<CropData>();
            turnipCrop.Id = "crop_turnip";
            turnipCrop.Name = "Turnip";
            turnipCrop.SeedItemId = "item_turnip_seeds";
            turnipCrop.GrowDays = 4;
            turnipCrop.WaterNeeds = WaterNeeds.Daily;
            turnipCrop.Seasons = new[] { Season.Spring };
            turnipCrop.GrowthStages = 4;
            turnipCrop.HarvestItemId = "item_turnip";
            turnipCrop.HarvestQuantity = new HarvestQuantity(1, 3);
            turnipCrop.Regrows = false;
            turnipCrop.RegrowDays = 0;
            turnipCrop.SpriteHints = new[] { "crop_turnip_stage1", "crop_turnip_stage2", "crop_turnip_stage3", "crop_turnip_mature" };
            turnipCrop.Tags = new[] { "crop", "vegetable", "spring", "base" };
            SaveAsset(turnipCrop, "Assets/Data/Crops/Turnip.asset");

            Debug.Log("Generated 1 crop");
        }

        private static void GenerateMobs()
        {
            // Chicken
            var chicken = ScriptableObject.CreateInstance<MobData>();
            chicken.Id = "mob_chicken";
            chicken.Name = "Chicken";
            chicken.Behavior = MobBehavior.Grazing;
            chicken.SpawnZones = new[] { Zone.Farm };
            chicken.SpawnWeight = 1f;
            chicken.SpawnWeather = new[] { Weather.Any };
            chicken.SpawnSeasons = new[] { Season.Spring, Season.Summer, Season.Fall, Season.Winter };
            chicken.Drops = new[] { new DropEntry("item_feather", 0.5f, 1, 2) };
            chicken.Harvestable = true;
            chicken.HarvestResultId = "item_egg";
            chicken.HarvestCooldownHours = 24f;
            chicken.PurchasableFromNPCId = "npc_farmer_jane";
            chicken.PurchasePrice = 100;
            chicken.SpriteHint = "mob_chicken";
            chicken.Tags = new[] { "mob", "animal", "farm", "base" };
            SaveAsset(chicken, "Assets/Data/Mobs/Chicken.asset");

            Debug.Log("Generated 1 mob");
        }

        private static void GenerateBuildings()
        {
            // Jane's Homestead
            var janesHome = ScriptableObject.CreateInstance<BuildingData>();
            janesHome.Id = "building_janes_homestead";
            janesHome.Name = "Jane's Homestead";
            janesHome.Zone = Zone.Village;
            janesHome.SizeTiles = new TileSize(4, 3);
            janesHome.DoorTile = new TilePosition(1, 0);
            janesHome.InteriorSize = InteriorSize.Medium;
            janesHome.InteriorLayout = new string[] { };
            janesHome.OwnerNPCId = "npc_farmer_jane";
            janesHome.EntryConditions = new[]
            {
                EntryCondition.Time(TimePhase.Day),
                EntryCondition.Friendship("npc_farmer_jane", 40)
            };
            janesHome.SpriteHint = "building_farmhouse";
            janesHome.Tags = new[] { "building", "home", "shop", "base" };
            SaveAsset(janesHome, "Assets/Data/Buildings/JanesHomestead.asset");

            Debug.Log("Generated 1 building");
        }

        private static void GenerateNPCs()
        {
            // Farmer Jane
            var jane = ScriptableObject.CreateInstance<NPCData>();
            jane.Id = "npc_farmer_jane";
            jane.Name = "Farmer Jane";
            jane.HomeBuildingId = "building_janes_homestead";
            jane.Schedule = GenerateDefaultSchedule(Zone.Village, Zone.Farm);
            jane.ShopInventory = new[] { "item_turnip_seeds", "item_hay" };
            jane.ShopSellPrices = new[]
            {
                new ShopPriceEntry("item_turnip_seeds", 1.0f),
                new ShopPriceEntry("item_hay", 1.0f)
            };
            jane.BuyPreferences = new[]
            {
                new BuyPreferenceEntry("item_turnip", 20),
                new BuyPreferenceEntry("item_egg", 15)
            };
            jane.GiftLikes = new[] { "item_turnip", "item_egg" };
            jane.GiftDislikes = new[] { "item_iron_ore" };
            jane.ActivityPreferences = new[]
            {
                new ActivityPreferenceEntry("activity_fishing", 1.5f)
            };
            jane.DialogueTags = new[] { "friendly", "hardworking" };
            jane.WeatherBehavior = new[]
            {
                WeatherBehaviorEntry.StaysIndoorsDuring(Weather.Rain)
            };
            jane.SpriteHint = "npc_farmer_female";
            jane.Tags = new[] { "npc", "farmer", "base", "starter" };
            SaveAsset(jane, "Assets/Data/NPCs/FarmerJane.asset");

            Debug.Log("Generated 1 NPC");
        }

        private static ScheduleEntry[] GenerateDefaultSchedule(Zone homeZone, Zone workZone)
        {
            var schedule = new ScheduleEntry[21]; // 7 days x 3 phases
            int index = 0;

            foreach (Weekday day in System.Enum.GetValues(typeof(Weekday)))
            {
                // Morning: at home, sleeping
                schedule[index++] = new ScheduleEntry(day, TimePhase.Morning, homeZone, NPCActivity.Sleeping);
                // Day: at work
                schedule[index++] = new ScheduleEntry(day, TimePhase.Day, workZone, NPCActivity.Working);
                // Night: back home
                schedule[index++] = new ScheduleEntry(day, TimePhase.Night, homeZone, NPCActivity.Idle);
            }

            return schedule;
        }

        private static void GenerateQuests()
        {
            // Bring Turnips
            var bringTurnips = ScriptableObject.CreateInstance<QuestData>();
            bringTurnips.Id = "quest_bring_turnips";
            bringTurnips.Name = "Fresh Produce";
            bringTurnips.GiverNPCId = "npc_farmer_jane";
            bringTurnips.Description = "Farmer Jane needs some fresh turnips for her stall. Bring her 5 turnips.";
            bringTurnips.QuestType = QuestType.Fetch;
            bringTurnips.Objectives = new[]
            {
                QuestObjective.Collect("item_turnip", 5)
            };
            bringTurnips.Rewards = new QuestRewards
            {
                Gold = 100,
                Items = new[] { new ItemReward("item_turnip_seeds", 10) },
                Friendship = new[] { new FriendshipReward("npc_farmer_jane", 15) }
            };
            bringTurnips.DeadlineDays = 0;
            bringTurnips.Repeatable = true;
            bringTurnips.Tags = new[] { "quest", "fetch", "farming", "starter" };
            SaveAsset(bringTurnips, "Assets/Data/Quests/FreshProduce.asset");

            Debug.Log("Generated 1 quest");
        }

        private static void GenerateActivities()
        {
            // Fishing
            var fishing = ScriptableObject.CreateInstance<ActivityData>();
            fishing.Id = "activity_fishing";
            fishing.Name = "Fishing";
            fishing.ValidZones = new[] { Zone.Beach };
            fishing.ValidPhases = new[] { TimePhase.Morning, TimePhase.Day };
            fishing.FriendshipRequired = 20;
            fishing.NPCEnjoyment = new[]
            {
                new NPCEnjoymentEntry("npc_farmer_jane", 1.5f)
            };
            fishing.FriendshipGainBase = 10;
            fishing.AdvancesTimePhase = true;
            fishing.Rewards = new[]
            {
                new ActivityRewardEntry("item_fish", 0.8f)
            };
            fishing.SpriteHint = "activity_fishing";
            fishing.Tags = new[] { "activity", "outdoor", "relaxing" };
            SaveAsset(fishing, "Assets/Data/Activities/Fishing.asset");

            Debug.Log("Generated 1 activity");
        }

        private static void GenerateEntities()
        {
            // Basic Horse
            var horse = ScriptableObject.CreateInstance<EntityData>();
            horse.Id = "entity_basic_horse";
            horse.Name = "Horse";
            horse.EntityType = EntityType.Mount;
            horse.SpeedMultiplier = 2f;
            horse.FuelItemId = "item_hay";
            horse.FuelConsumptionRate = 1f;
            horse.FuelBonusEffect = "2.5x speed for 1 day when fed";
            horse.PurchasePrice = 500;
            horse.SpriteHint = "entity_horse";
            horse.Tags = new[] { "entity", "mount", "animal" };
            SaveAsset(horse, "Assets/Data/Entities/BasicHorse.asset");

            Debug.Log("Generated 1 entity");
        }

        private static void GenerateDialogueOptions()
        {
            // Ask About Weather
            var askWeather = ScriptableObject.CreateInstance<DialogueOptionData>();
            askWeather.Id = "dialogue_ask_weather";
            askWeather.Text = "How's the weather treating you?";
            askWeather.Conditions = new DialogueCondition[] { };
            askWeather.Effects = new[]
            {
                DialogueEffect.ChangeFriendship("", 1)
            };
            askWeather.ResponseDialogue = "Oh, it's been lovely! Perfect for the crops.";
            askWeather.Tags = new[] { "dialogue", "smalltalk", "weather" };
            SaveAsset(askWeather, "Assets/Data/DialogueOptions/AskAboutWeather.asset");

            Debug.Log("Generated 1 dialogue option");
        }

        private static void SaveAsset(ScriptableObject asset, string path)
        {
            // Ensure directory exists
            string directory = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            AssetDatabase.CreateAsset(asset, path);
        }
    }
}
