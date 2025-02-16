using UnityEngine;

namespace Character
{
    public class M_CR_Ball : Monster
    {
        private bool attackReady;
        private float rollTime;

        protected override void Awake()
        {
            base.Awake();
            ai.stoppingDistance = 0.1f;
        }

        private void OnDestroy()
        {
            if (Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength * 0.5f, colliders, 1 << 3) <=
                0) return;
            colliders[0].TryGetComponent(out targetCharacter);
            targetCharacter.Hit(thisCurTransform.position, characterStat.maxHP * 0.5f, 0);
        }

        public void Roll()
        {
            attackReady = true;
            ai.speed = speed +50;
            def += 3;
            rollTime = 4;
            AudioManager.Instance.PlayEffect((int)CombatEffectClip.rollingBall,weapon);
        }

        protected void Update()
        {
            if (!BaseUpdate())
                return;


            if (target)
            {
                if (attackReady)
                {
                    rollTime -= Time.deltaTime;
                    if (rollTime < 0)
                    {
                        target = null;
                        def -= 3;
                        ai.speed = speed;
                        attackReady = false;
                        weapon.Stop();
                        return;
                    }
                }
               
                Vector3 targetPosition = target.position;
                if (attackReady)
                {
                    float targetDistance = Vector3.Distance(targetPosition, thisCurTransform.position);

                    ai.SetDestination(targetPosition);
                    if (targetDistance > sightLength || targetDistance <= range)
                    {                        
                        if (targetDistance <= range)
                        {
                            target.gameObject.TryGetComponent(out targetCharacter);
                            AudioManager.Instance.PlayEffect((int)CombatEffectClip.crash,weapon);
                            targetCharacter.Hit(thisCurTransform.position, dmg, 0);
                        }
                        target = null;
                        def -= 3;
                        ai.speed = speed;
                        attackReady = false;
                    }
                }
            }
            else
            {
                if (!trackingPermission) return;                
                int size = Physics.OverlapSphereNonAlloc(thisCurTransform.position, sightLength, colliders, 1 << 3);
                if (size > 0)
                {
                    float angle = Mathf.Acos(Vector3.Dot(thisCurTransform.forward, (colliders[0].transform.position - thisCurTransform.position).normalized)) * Mathf.Rad2Deg;

                    if ((angle < 0 ? -angle : angle) < viewAngle ||
                        Vector3.Distance(colliders[0].transform.position, thisCurTransform.position) <
                        sightLength * 0.4f)
                    {
                        target = colliders[0].transform;
                    }
                }
            }

        }
    }
}