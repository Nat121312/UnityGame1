using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Entity
{
    [SerializeField] EntitiesBase _base;
    public EntitiesBase Base {
        get { return _base; }
    }
    public int currentHP { get; set; }
    public float currentMP { get; set; }
    public List<Move> Moves { get; set; }

    public void Init() {
            currentHP = MaxHP;
            currentMP = Magicules;
            // Generate Moves
            Moves = new List<Move>();
            foreach (var move in Base.LearnableMoves) {
                if (move.MagiculeNeeded <= Magicules) {
                    Moves.Add(new Move(move.Base));
                }

                if (Moves.Count >= 4) {
                    break;
                }
            }
    }

        public int Attack {
            get { return Mathf.FloorToInt((Base.Magicules / 50) * Base.Attack / 100f) + 5; }
        }

        public int MaxHP {
            get { return Mathf.FloorToInt((Base.Magicules / 50) * Base.MaxHP / 100f) + 10; }
        }

        public int Defense {
            get { return Mathf.FloorToInt((Base.Magicules / 50) * Base.Defense / 100f) + 5; }
        }

        public int MagicAttack {
            get { return Mathf.FloorToInt((Base.Magicules / 50) * Base.MagicAttack / 100f) + 5; }
        }

        public float Magicules {
            get { return Base.Magicules; }
        }

        public int Speed {
            get { return Mathf.FloorToInt((Base.Magicules / 50) * Base.Speed / 100f) + 5; }
        }

        public DamageDetails TakeDamage(Move move, Entity attacker) {
            float critic = 1f;
            if (Random.value * 100f <= 6.25) {
                critic = 2f;
            }

            float type = TypeChart.GetEffectivenes(move.Base.Type, this.Base.Type);

            var DamageDetails = new DamageDetails {
                TypeEffectiveness = type,
                Critic = critic,
                Fainted = false
            };

            float AttackStat = (move.Base.Origin == MoveOrigin.Magic) ? attacker.MagicAttack : attacker.Attack;

            float modifiers = Random.Range(0.8f, 1f) * type * critic;
            double e = Magicules*1.95 / 1000;
            float a = (2 * ((float)e) + 10) / 250f;
            float d = a * move.Base.Power * ((float)AttackStat / Defense) + 2;
            int damage = Mathf.FloorToInt(d * modifiers);

            currentHP -= damage;
            if (currentHP <= 0) {
                currentHP = 0;
                DamageDetails.Fainted = true;
            }
            return DamageDetails;
        }

        public Move GetRandomMove() {
            int random = Random.Range(0, Moves.Count);
            return Moves[random];
        }

        public bool UpdateMagiculeCount(Move move) {
            currentMP -= move.Base.MagiculeCost;
            if (currentMP <= 0) {
                currentMP += move.Base.MagiculeCost;
                return false;
            }
            return true;
        }

    }

    public class DamageDetails {
        public bool Fainted { get; set; }
        public float Critic { get; set; }
        public float TypeEffectiveness { get; set; }
    }
