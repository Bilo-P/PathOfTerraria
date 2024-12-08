using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathOfTerraria.Common.Mechanics;

namespace PathOfTerraria.Tests.Common.Mechanics
{
    [TestClass]
    public class SkillTest
    {
        public class TestSkill : Skill
        {
            public override List<SkillPassive> Passives => [];
            public override int MaxLevel => 5;

            public override void LevelTo(byte level)
            {
                Level = level;
            }

            public override void UseSkill(Player player)
            {
            }
        }

        [TestMethod]
        public void CanUseSkill_PlayerHasEnoughMana()
        {
            var skill = new TestSkill
            {
                Timer = 0,
                ManaCost = 20
            };

            var player = new Player
            {
                statMana = 30
            };

            bool result = skill.CanUseSkill(player);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanUseSkill_PlayerDoesNotHaveEnoughMana()
        {
            var skill = new TestSkill
            {
                Timer = 0,
                ManaCost = 50
            };

            var player = new Player
            {
                statMana = 30
            };

            bool result = skill.CanUseSkill(player);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LevelTo_SetsLevelCorrectly()
        {
            var skill = new TestSkill();
            skill.LevelTo(3);

            Assert.AreEqual(3, skill.Level);
        }
    }
}
