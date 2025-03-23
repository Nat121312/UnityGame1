using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity
{
    public EntitiesBase Base { get; set; }
    public int Level { get; set; }
    public int currentHP { get; set; }
    public int currentMP { get; set; }
    public List<Move> Moves { get; set; }

    public Entity(EntitiesBase pBase, int pLevel) {
            Base = pBase;
            Level = pLevel;
            currentHP = MaxHP;
            currentMP = Magicules;
            // Generate Moves
            Moves = new List<Move>();
            foreach (var move in Base.LearnableMoves) {
                if (move.Level <= Level) {
                    Moves.Add(new Move(move.Base));
                }

                if (Moves.Count >= 4) {
                    break;
                }
            }
    }

        public int Attack {
            get { return Mathf.FloorToInt(Base.Attack * Level / 100f) + 5; }
        }

        public int MaxHP {
            get { return Mathf.FloorToInt(Base.MaxHP * Level / 100f) + 10; }
        }

        public int Defense {
            get { return Mathf.FloorToInt(Base.Defense * Level / 100f) + 5; }
        }

        public int MagicAttack {
            get { return Mathf.FloorToInt(Base.MagicAttack * Level / 100f) + 5; }
        }

        public int Magicules {
            get { return Mathf.FloorToInt(Base.Magicules * Level / 7f) + 5; }
        }

        public int Speed {
            get { return Mathf.FloorToInt(Base.Speed * Level / 100f) + 5; }
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
            float a = (2 * attacker.Level + 10) / 250f;
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
