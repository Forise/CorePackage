//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Weapon2D : AAttackerPresenter
    {
        #region Fields
        [SerializeField]
        private string id;
        [SerializeField]
        private AWeaponModifier modifier;
        private Vector2 direction;
        #endregion Fields

        #region Properties
        public string ID { get => id; }
        #endregion Properties

        protected override void Init()
        {
            base.Init();
            if (animator)
                animator.SetFloat("AttackSpeed", AttackSpeed);
        }

        public void CopyFrom(Weapon2D weapon)
        {
            AnimationTriggers = weapon.AnimationTriggers;
            attackFromAnimation = weapon.attackFromAnimation;
            attackModel = weapon.attackModel;
            damageObjects_ToSpawn = weapon.damageObjects_ToSpawn;
            delay = weapon.delay;
            isAnimating = weapon.isAnimating;
            modifier = weapon.modifier;
            buffs = weapon.buffs;
        }

        protected override List<ADamagePresenter> SpawnDamageObjects()
        {
            if (directionPoint)
            {
                ADamagePresenter damageObj;

                float rotation_z;

                if(useRandomSpawnPoint)
                {
                    int i = Random.Range(0, spawnDirectionPoints.Count);
                    spawnPoint = spawnDirectionPoints.Keys.ElementAt(i);
                    directionPoint = spawnDirectionPoints[spawnPoint];
                }

                direction = directionPoint.position - (!spawnPoint ? transform.position : spawnPoint.position);
                direction.Normalize();
                rotation_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                List<ADamagePresenter> objs = new List<ADamagePresenter>();
                for (int i = 0; i < damageObjects_ToSpawn.Count; i++)
                {
                    damageObj = Instantiate(damageObjects_ToSpawn[i], (!spawnPoint ? transform.position : spawnPoint.position), Quaternion.Euler(0f, 0f, rotation_z));
                    damageObj.transform.parent = this.gameObject.transform;
                    damageObj.Damage = Random.Range(0f,100f) < attackModel.CriticalChance ? Damage * attackModel.CritMultiplyer : Damage;
                    objs.Add(damageObj);
                    damageObj.parent = gameObject;
                    switch (attackModel.Type)
                    {
                        case AttackerType.Range:
                            var objMovement = damageObj.gameObject.GetComponent<MovementPresenter2D>();
                            if (objMovement)
                            {
                                objMovement.SetPosition(damageObj.transform.position);
                                objMovement.SetDirection(direction);
                                objMovement.SetSpeed(attackModel.Power);
                            }
                            break;
                        //case AttackerType.RangeBezier:
                        //    var follower = damageObj.gameObject.GetComponent<PathCreation.Examples.PathFollower>();
                        //    if (follower)
                        //    {
                        //        follower.pathCreator = pathCreators[i];
                        //        follower.speed = attackModel.Power;
                        //        follower.MovementStoped += () => { Destroy(follower.gameObject); };
                        //        objs.Add(damageObj);
                        //    }
                        //    else
                        //    {
                        //        Debug.LogError($"Follower is null! I:[{i}]", this);
                        //    }
                        //    break;
                        case AttackerType.Spline:
                            var spline = damageObj.gameObject.GetComponent<SplineController>();
                            damageObj.gameObject.SetActive(true);
                            break;
                        default:
                            break;
                    }
                }
                foreach(var e in EventsToNotify_OnAttack)
                {
                    EventSystem.EventManager.Notify(this, new EventSystem.GameEventArgs(e));
                }

                return objs;
            }
            else
            {
                Debug.LogError("Direction point is null! Cant instantiate attack!", this.gameObject);
                return null;
            }
        }

        protected override void UpdateAttackSpeedView()
        {
            animator.SetFloat("AttackSpeed", attackModel.AttackSpeed);
        }
    }
}