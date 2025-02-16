using Character;
using Skill;
using UnityEngine;

namespace Item
{
    public class UsingItem : MonoBehaviour, IItem
    {
        public ItemType type;

        private SPC[] spcs;

        static ItemInfo[] infos = new ItemInfo[4];
        float[] temps = new float[2];
        static decimal spdValue = 0.05m;
        static decimal dmgValue = 2;
        private void Awake()
        {
            for (int i = 0; i < ResourceManager.Instance.itemInfos.Length; i++)
            {
                infos[i] = ResourceManager.Instance.itemInfos[i];
            }

            spcs = new SPC[3]
            {
                new((ch) =>
                {
                    ch.hp += 40;
                    ch.MaxHp += 40;
                    staticStat.maxHP += 40;
                    MapInfo.hpCore++;
                },(ch) => spcs[0].Tick((stack)=>{ch.hp+=stack* ch.characterStat.maxHP*0.01f; }),null,infos[0].SPC_Sprite),
                new( (ch) => {
                    staticStat.speed = ch.speed = (float)((decimal)ch.speed + spdValue);
                    MapInfo.speedCore++;
                     temps[0]= ch.speed;
                     ch.speed+= temps[0]*0.2f;
                 }, (ch) =>ch.speed-=temps[0]*0.2f, infos[1].SPC_Sprite),
                 new((ch) => {
                    staticStat.dmg = ch.dmg = (float)((decimal)ch.dmg + dmgValue);
                    MapInfo.dmgCore++;
                     temps[1]= ch.dmg;
                     ch.dmg += temps[1] * 0.2f;
                 },(ch)=>ch.dmg-=temps[1]*0.2f , infos[2].SPC_Sprite),
            };
            SpawnManager.Instance.itemPool.Add(this);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Player player)
        {
            ReleaseEffect effect = SpawnManager.Instance.GetEffect(player.transform.position, infos[(int)type].targetParticle, (int)CombatEffectClip.itemUse, 1, 20);
            effect.transform.SetParent(player.transform, true);
            spcs[(int)type].Init(20);

            player.AddBuff(spcs[(int)type]);

            MapInfo.core++;
            UIManager.Instance.playerStatUIController.core = MapInfo.core;
            gameObject.SetActive(false);
        }
    }
}
//switch (type)
//{
//    case ItemType.Heal:
//        player.hp += 40;
//        player.MaxHp += 40;
//        Character.staticStat.maxHP += 40;
//        MapInfo.hpCore++;
//        break;
//    case ItemType.Boost:
//        // decimal 자료형으로 연산 후 다시 float로 형변환
//        player.speed = (float)((decimal)player.speed + spdValue);
//        Character.staticStat.speed = (float)((decimal)player.speed + spdValue);
//        MapInfo.speedCore++;
//        break;
//    case ItemType.PowerUp:
//        player.dmg = (float)((decimal)player.dmg + dmgValue);
//        Character.staticStat.dmg = (float)((decimal)player.dmg + dmgValue);
//        MapInfo.dmgCore++;
//        break;
//}