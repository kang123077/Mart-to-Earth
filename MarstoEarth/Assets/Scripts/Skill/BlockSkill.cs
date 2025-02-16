using System;
using UnityEngine;

namespace Skill
{
    public class BlockSkill : Skill
    {
        private SPC block;
        private SPC parring;
        private bool parrying;
        private Func<Vector3, float, float, bool> temp;
        private ParticleSystem effect;
        private ParticleSystem enforceEffect;
        private AudioClip[] clips;
        public BlockSkill()
        {
            skillInfo = ResourceManager.Instance.skillInfos[(int)SkillName.Block];
            block = new SPC((ch) =>
            {
                temp = ch.Hit;
                ch.Hit = (attacker, dmg, penetrate) =>
                {
                    if (!ch.Hited(attacker, dmg * 0.05f, penetrate)) return false;
                    if (parrying || Physics.OverlapSphereNonAlloc(ch.transform.position,enforce? skillInfo.range*2:skillInfo.range, caster.colliders, ch.layerMask) < 1) return true;
                    if(enforce)
                        enforceEffect.Play();
                    else
                        effect.Play();
                    
                    
                    AudioManager.Instance.PlayEffect((int)CombatEffectClip.parryingKick, ch.weapon);
                    attacker = caster.colliders[0].transform.position;
                    attacker.y = ch.transform.position.y;
                    ch.transform.LookAt(attacker);
                    ch.anim.SetBool($"parring", parrying = true);
                    return true;
                };
            }, (ch) =>
            {
                ch.Hit = temp;
                if (parrying) return;
                ch.onSkill = null;
            }, skillInfo.icon);
            parring = new SPC((ch) => ch.stun = true,
                (ch) => ch.stun = false, ResourceManager.Instance.commonSPCIcon[(int)CommonSPC.stun]);
        }

        public override void Init(Character.Character caster)
        {
            base.Init(caster);
            effect = UnityEngine.Object.Instantiate(skillInfo.effects[0], caster.transform);
            enforceEffect = UnityEngine.Object.Instantiate(skillInfo.effects[1], caster.transform);
        }

        protected override bool Activate()
        {
            caster.anim.SetBool($"parring", parrying = false);
            block.Init(skillInfo.duration + caster.duration * 0.1f);
            caster.PlaySkillClip(this);
            caster.AddBuff(block);

            return true;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Effect()
        {
            effect.Stop();
            enforceEffect.Stop();
            Vector3 transPos = caster.transform.position;
            int size = Physics.OverlapSphereNonAlloc(transPos,enforce?(skillInfo.range + caster.range * 0.2f)*2 :skillInfo.range + caster.range * 0.2f, caster.colliders, caster.layerMask);
            if (size < 1) return;

            for (int i = 0; i < size; i++)
            {
                caster.colliders[i].TryGetComponent(out caster.targetCharacter);
                if (caster.targetCharacter)
                {
                    if (!caster.targetCharacter.Hit(transPos, skillInfo.dmg + caster.dmg * 2f, 0)) return;
                    parring.Init(skillInfo.duration + caster.duration * 0.2f);
                    caster.targetCharacter.AddBuff(parring);
                    caster.targetCharacter.impact -= caster.targetCharacter.transform.forward * 3;
                }
            }

        }
    }
}