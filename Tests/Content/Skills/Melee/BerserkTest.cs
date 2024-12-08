using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathOfTerraria.Common.Enums;
using PathOfTerraria.Content.Skills.Melee;

namespace PathOfTerraria.Tests.Content.Skills.Melee
{
    [TestClass]
    [TestSubject(typeof(Berserk))]
    public class BerserkTest
    {

        [TestMethod]
        public void LevelTo_SetsCorrectValues()
        {
            var skill = new Berserk();
            skill.LevelTo(2);

            Assert.AreEqual(2, skill.Level);
            Assert.AreEqual(3000, skill.Cooldown);
            Assert.AreEqual(3000, skill.MaxCooldown);
            Assert.AreEqual(20, skill.ManaCost);
            Assert.AreEqual(1500, skill.Duration);
            Assert.AreEqual(ItemType.Sword, skill.WeaponType);
        }
        
        [TestMethod]
        public void CanUseSkill_PlayerDoesNotHaveEnoughMana()
        {
            var player = new Player
            {
                statMana = 30
            };

            var skill = new Berserk { ManaCost = 50 };

            bool result = skill.CanUseSkill(player);

            Assert.IsFalse(result);
        }
    }
}

