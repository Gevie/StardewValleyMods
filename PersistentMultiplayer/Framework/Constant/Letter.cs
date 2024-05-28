using System.ComponentModel;

namespace PersistentMultiplayer.Framework.Constant
{
    internal enum Letter
    {
        // TODO: Re-evaluate this approach... 
        // Usage Idea: HostCharacterMail.Has(Letter.FishingRodFromWilly)
        // Usage Idea: HostCharacterMail.Has(dictionaryOfEnumValues)
        // Supported By: EnumHelper.GetDescription(Letter.FishingRodFromWilly) -> spring_2_1
        
        // Host Specific Event Triggers
        [Description("spring_2_1")] FishingRodFromWilly,
        [Description("TBD")] MeetTheWizard,
        [Description("TBD")] NonDatableVillagersPlusTwo,
        [Description("TBD")] FarmingLevelTenAchieved,
        [Description("TBD")] ObtainAllRarecrows,
        [Description("TBD")] CompleteIslandIngredients,
        [Description("TBD")] CompleteCavePatrol,
        [Description("TBD")] CompleteAquaticOverpopulationBiomeBalance,
        [Description("TBD")] CompleteFragmentsOfThePast,
        [Description("TBD")] CompleteCommunityCleanup,
        [Description("TBD")] CompleteTheStrongStuff,
        [Description("TBD")] CompleteRobinResourceRush,
        [Description("TBD")] CompleteJuicyBugsWanted,
        [Description("TBD")] CompleteACuriousSubstance,
        [Description("TBD")] CompletePrismaticJelly,
        
        // Have Host Receive Gold
        [Description("TBD")] FiveThousandGoldEarnedFemale,
        [Description("TBD")] FifteenThousandGoldEarnedMale,
        [Description("TBD")] FifteenThousandGoldEarnedFemale,
        [Description("TBD")] SkullCavernFloorTwentyFiveReached,
        [Description("TBD")] CompleteTenHelpWantedQuests,
        [Description("TBD")] CompleteThirtyFiveHelpWantedQuests,
        [Description("TBD")] LewisGoldStatuePlaced,
        
        // Have Host Deposit Item For Farmhands
        [Description("TBD")] FiveThousandGoldEarnedMale,
        [Description("TBD")] OneHundredTwentyThousandGoldEarnedMale,
        [Description("TBD")] OneHundredTwentyThousandGoldEarnedFemale,
        [Description("TBD")] BeatJourneyOfThePrairieKing,
        [Description("TBD")] BeatJunimoKart,
        [Description("TBD")] RepeatCavePatrol,
        [Description("TBD")] CompleteRockRejuvenation,
        [Description("TBD")] CompleteGiftsForGeorge,
        [Description("TBD")] CompleteGusFamousOmelet,
        [Description("TBD")] CompleteCropOrder,
        [Description("TBD")] CompletePierrePrimeProduce,
        [Description("TBD")] CompleteTropicalFish,
        
        // Notify Farmhands
        [Description("TBD")] CoopUpgraded,
        [Description("TBD")] PierreOpenOnWednesdays,
        [Description("TBD")] AccessToBoatRoom,
        [Description("TBD")] AccessToGingerIsland,
        [Description("TBD")] CanPurchasePerfectionWaivers,
    }
}