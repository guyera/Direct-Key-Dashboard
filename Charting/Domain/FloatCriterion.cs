using System;
using Newtonsoft.Json.Linq;

namespace DirectKeyDashboard.Charting.Domain {
    // Representation of a floating point number
    // criterion, i.e. some object's floating
    // point property must be within some
    // range specified by this criterion,
    // else it may be filtered out.
    public class FloatCriterion : Criterion {
        private const float Precision = 0.01f;
        public string Key {get; set;}
        public float Value {get; set;}
        public Relation ValueRelation {get; set;}

        // For model binding
        public FloatCriterion() : base(typeof(FloatCriterion).FullName) {}

        public FloatCriterion(string key, float value, Relation valueRelation) : base(typeof(FloatCriterion).FullName) {
            Key = key;
            Value = value;
            ValueRelation = valueRelation;
        }

        public override bool SatisfiedBy(JObject jobject) {
            if (!jobject.TryGetValue(Key, out var token)) {
                return false;
            }

            var value = (float?) token.ToObject(typeof(float));
            if (value == null) {
                return false;
            }

            switch(ValueRelation) {
                case Relation.Equal:
                    return Math.Abs(value.Value - Value) <= Precision;
                case Relation.NotEqual:
                    return Math.Abs(value.Value - Value) > Precision;
                case Relation.Greater:
                    return value > Value;
                case Relation.GreaterOrEqual:
                    return value >= Value;
                case Relation.Less:
                    return value < Value;
                case Relation.LessOrEqual:
                    return value <= Value;
                default:
                    return false;
            }
        }

        public enum Relation{
            Equal,
            NotEqual,
            Greater,
            Less,
            GreaterOrEqual,
            LessOrEqual
        }
    }
}