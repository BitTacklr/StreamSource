using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace StreamSource.Framework
{
    public class EqualityAssertion
    {
        private readonly EqualityAssertionOptions _options;
        private readonly IEqualityTestCase[] _cases;

        public EqualityAssertion(object instance, EqualityAssertionOptions options = null)
        {
            if (instance == null) 
                throw new ArgumentNullException("instance");
            _options = options ?? new EqualityAssertionOptions();
            var cases = new List<IEqualityTestCase>();
            if (_options.VerifyObjectEquals)
            {
                cases.Add(new ObjectEqualsTestCase(instance, null, false));
                cases.Add(new ObjectEqualsTestCase(instance, instance, true));
            }
            if (_options.VerifyTypeEquals)
            {
                cases.Add(new TypeEqualsTestCase(instance, null, false));
                cases.Add(new TypeEqualsTestCase(instance, instance, true));
            }
            if (_options.VerifyEqualsOperator)
            {
                cases.Add(new EqualsOperatorTestCase(instance, null, false));
                cases.Add(new EqualsOperatorTestCase(instance, instance, true));
            }
            if (_options.VerifyNotEqualsOperator)
            {
                cases.Add(new NotEqualsOperatorTestCase(instance, null, true));
                cases.Add(new NotEqualsOperatorTestCase(instance, instance, false));
            }
            _cases = cases.ToArray();
        }

        EqualityAssertion(EqualityAssertionOptions options, IEqualityTestCase[] cases)
        {
            _options = options;
            _cases = cases;
        }

        public EqualityAssertion VerifyEqual(object left, object right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            var cases = new List<IEqualityTestCase>();
            if (_options.VerifyObjectEquals) cases.Add(new ObjectEqualsTestCase(left, right, true));
            if (_options.VerifyTypeEquals) cases.Add(new TypeEqualsTestCase(left, right, true));
            if (_options.VerifyGetHashCode) cases.Add(new GetHashCodeTestCase(left, right, true));
            if (_options.VerifyEqualsOperator) cases.Add(new EqualsOperatorTestCase(left, right, true));
            if (_options.VerifyNotEqualsOperator) cases.Add(new NotEqualsOperatorTestCase(left, right, false));
            return new EqualityAssertion(_options, _cases.Concat(cases).ToArray());
        }

        public EqualityAssertion VerifyNotEqual(object left, object right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            var cases = new List<IEqualityTestCase>();
            if (_options.VerifyObjectEquals) cases.Add(new ObjectEqualsTestCase(left, right, false));
            if (_options.VerifyTypeEquals && left.GetType() == right.GetType()) cases.Add(new TypeEqualsTestCase(left, right, false));
            if (_options.VerifyGetHashCode) cases.Add(new GetHashCodeTestCase(left, right, false));
            if (_options.VerifyEqualsOperator && left.GetType() == right.GetType()) cases.Add(new EqualsOperatorTestCase(left, right, false));
            if (_options.VerifyNotEqualsOperator && left.GetType() == right.GetType()) cases.Add(new NotEqualsOperatorTestCase(left, right, true));
            return new EqualityAssertion(_options, _cases.Concat(cases).ToArray());
        }

        public void Assert()
        {
            foreach (var @case in _cases)
            {
                @case.Assert();
            }
        }

        interface IEqualityTestCase
        {
            void Assert();
        }

        class ObjectEqualsTestCase : IEqualityTestCase
        {
            private readonly object _left;
            private readonly object _right;
            private readonly bool _result;

            public ObjectEqualsTestCase(object left, object right, bool result)
            {
                _left = left;
                _right = right;
                _result = result;
            }

            public void Assert()
            {
                var actual = _left.Equals(_right);
                NUnit.Framework.Assert.That(actual, Is.EqualTo(_result), 
                    "{0}.Equals((object) other) failed",
                    _left.GetType().Name);
            }
        }

        class TypeEqualsTestCase : IEqualityTestCase
        {
            private readonly object _left;
            private readonly object _right;
            private readonly bool _result;

            public TypeEqualsTestCase(object left, object right, bool result)
            {
                _left = left;
                _right = right;
                _result = result;
            }

            public void Assert()
            {
                if (_right == null || _right.GetType() == _left.GetType())
                {
                    var method = _left.GetType().GetMethods().
                        SingleOrDefault(_ =>
                            _.Name == "Equals" &&
                            _.ReturnType == typeof(bool) &&
                            _.GetParameters().Count() == 1 &&
                            _.GetParameters().Single().ParameterType == _left.GetType());
                    if (method == null)
                    {
                        NUnit.Framework.Assert.Fail(
                            "Method not found: {0}.Equals({0} other)", 
                            _left.GetType().Name);
                    }
                    else
                    {
                        var actual = (bool) method.Invoke(_left, new[] {_right});
                        NUnit.Framework.Assert.That(actual, Is.EqualTo(_result), 
                            "{0}.Equals(({0}) other) failed",
                            _left.GetType().Name);
                    }
                }
                else
                {
                    var method = _left.GetType().GetMethods().
                        SingleOrDefault(_ =>
                            _.Name == "Equals" &&
                            _.ReturnType == typeof (bool) &&
                            _.GetParameters().Count() == 1 &&
                            _.GetParameters().Single().ParameterType == _right.GetType());
                    if (method == null)
                    {
                        NUnit.Framework.Assert.Fail(
                            "Method not found: {0}.Equals({1} other)",
                            _left.GetType().Name,
                            _right.GetType().Name);
                    }
                    else
                    {
                        var actual = (bool)method.Invoke(_left, new[] { _right });
                        NUnit.Framework.Assert.That(actual, Is.EqualTo(_result), 
                            "{0}.Equals(({1}) other) failed",
                            _left.GetType().Name,
                            _right.GetType().Name);
                    }
                }
            }
        }

        class GetHashCodeTestCase : IEqualityTestCase
        {
            private readonly object _left;
            private readonly object _right;
            private readonly bool _result;

            public GetHashCodeTestCase(object left, object right, bool result)
            {
                _left = left;
                _right = right;
                _result = result;
            }

            public void Assert()
            {
                var actual = _left.GetHashCode().Equals(_right.GetHashCode());
                NUnit.Framework.Assert.That(actual, Is.EqualTo(_result), 
                    "{0}.GetHashCode().Equals({1}.GetHashCode()) failed",
                    _left.GetType().Name,
                    _right.GetType().Name);
            }
        }

        class EqualsOperatorTestCase : IEqualityTestCase
        {
            private readonly object _left;
            private readonly object _right;
            private readonly bool _result;

            public EqualsOperatorTestCase(object left, object right, bool result)
            {
                _left = left;
                _right = right;
                _result = result;
            }

            public void Assert()
            {
                if (_right == null || _right.GetType() == _left.GetType())
                {
                    var method = _left.GetType().GetMethod("op_Equality", new[] { _left.GetType(), _left.GetType() });
                    var actual = (bool)method.Invoke(null, new[] { _left, _right });
                    NUnit.Framework.Assert.That(actual, Is.EqualTo(_result),
                        "{0} == {1} failed",
                        _left.GetType().Name,
                        _right == null ? _left.GetType().Name : _right.GetType().Name);
                }
                else
                {
                    var method = _left.GetType().GetMethod("op_Equality", new[] { _left.GetType(), _right.GetType() });
                    var actual = (bool)method.Invoke(null, new[] { _left, _right });
                    NUnit.Framework.Assert.That(actual, Is.EqualTo(_result),
                        "{0} == {1} failed",
                        _left.GetType().Name,
                        _right.GetType().Name);
                }
            }
        }

        class NotEqualsOperatorTestCase : IEqualityTestCase
        {
            private readonly object _left;
            private readonly object _right;
            private readonly bool _result;

            public NotEqualsOperatorTestCase(object left, object right, bool result)
            {
                _left = left;
                _right = right;
                _result = result;
            }

            public void Assert()
            {
                if (_right == null || _right.GetType() == _left.GetType())
                {
                    var method = _left.GetType().GetMethod("op_Inequality", new[] { _left.GetType(), _left.GetType() });
                    var actual = (bool)method.Invoke(null, new[] { _left, _right });
                    NUnit.Framework.Assert.That(actual, Is.EqualTo(_result),
                        "{0} != {1} failed",
                        _left.GetType().Name,
                        _right == null ? _left.GetType().Name : _right.GetType().Name);
                }
                else
                {
                    var method = _left.GetType().GetMethod("op_Inequality", new[] { _left.GetType(), _right.GetType() });
                    var actual = (bool)method.Invoke(null, new[] { _left, _right });
                    NUnit.Framework.Assert.That(actual, Is.EqualTo(_result),
                        "{0} != {1} failed",
                        _left.GetType().Name,
                        _right.GetType().Name);
                }
            }
        }
    }
}
