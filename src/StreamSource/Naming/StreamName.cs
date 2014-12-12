using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamSource.Naming
{
    public class StreamName
    {
        public static readonly StreamName Empty = new StreamName(new StreamNameComponent[0]);

        private readonly StreamNameComponent[] _components;

        public StreamName(StreamNameComponent[] components)
        {
            if (components == null) 
                throw new ArgumentNullException("components");
            _components = components;
        }

        public StreamNameComponent[] Components
        {
            get { return _components; }
        }

        public StreamName Append(StreamNameComponent component)
        {
            if (component == null) throw new ArgumentNullException("component");
            var combined = new StreamNameComponent[_components.Length + 1];
            _components.CopyTo(combined, 0);
            combined[_components.Length] = component;
            return new StreamName(combined);
        }

        public StreamName Prepend(StreamNameComponent component)
        {
            if (component == null) throw new ArgumentNullException("component");
            var combined = new StreamNameComponent[_components.Length + 1];
            combined[0] = component;
            _components.CopyTo(combined, 1);
            return new StreamName(combined);
        }

        public StreamNameComponent[] this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException("name");
                return _components.
                    Where(component => string.Equals(component.Name, name, StringComparison.Ordinal)).
                    ToArray();
            }
        }

        public bool Equals(StreamName other)
        {
            return !ReferenceEquals(null, other) && _components.SequenceEqual(other._components);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is StreamName && Equals((StreamName)obj);
        }

        public override int GetHashCode()
        {
            return _components.Aggregate(0, (current, next) => current ^ next.GetHashCode());
        }

        public override string ToString()
        {
            return String.Join(new string(StreamNameFormat.ComponentSeparator, 1), _components.Select(_ => _.ToString()));
        }

        public static implicit operator String(StreamName instance)
        {
            return instance.ToString();
        }

        public static bool operator ==(StreamName left, StreamName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StreamName left, StreamName right)
        {
            return !Equals(left, right);
        }

        public static StreamName Parse(string value)
        {
            if (value == null) 
                throw new ArgumentNullException("value");

            return value.
                Aggregate(
                    ParseContext.Initial,
                    (context, @char) => StateHandlers[context.State](context, @char).NextPosition(),
                    context => context.Complete());
        }

        private static readonly IReadOnlyDictionary<ParseState, Func<ParseContext, Char, ParseContext>> StateHandlers =
            new Dictionary<ParseState, Func<ParseContext, char, ParseContext>>
            {
                {
                    ParseState.Initial, (context, @char) =>
                    {
                        if (@char == StreamNameFormat.Escape)
                        {
                            return context.TransitionTo(ParseState.ComponentNameEscaping);
                        }
                        if (@char == StreamNameFormat.ComponentSeparator)
                        {
                            throw new FormatException(string.Format("The component separator ({0}) was not properly escaped (position: {1}).", StreamNameFormat.ComponentSeparator, context.Position));
                        }
                        if (@char == StreamNameFormat.ComponentNameValueSeparator)
                        {
                            throw new FormatException(string.Format("The component name value separator ({0}) was not properly escaped (position: {1}).", StreamNameFormat.ComponentNameValueSeparator, context.Position));
                        }
                        return context.AppendToComponentName(@char).TransitionTo(ParseState.ComponentName);
                    }
                },
                {
                    ParseState.ComponentName, (context, @char) =>
                    {
                        if (@char == StreamNameFormat.Escape)
                        {
                            return context.TransitionTo(ParseState.ComponentNameEscaping);
                        }
                        if (@char == StreamNameFormat.ComponentSeparator)
                        {
                            throw new FormatException(string.Format("The component separator ({0}) was not properly escaped (position: {1}).", StreamNameFormat.ComponentSeparator, context.Position));
                        }
                        if (@char == StreamNameFormat.ComponentNameValueSeparator)
                        {
                            if (context.ComponentName.Length == 0)
                            {
                                throw new FormatException(string.Format("The component name cannot be empty (position: {0}). Please specify a name.", context.Position));
                            }
                            return context.TransitionTo(ParseState.ComponentValue);
                        }
                        return context.AppendToComponentName(@char);
                    }
                },
                {
                    ParseState.ComponentNameEscaping, (context, @char) =>
                    {
                        if (StreamNameFormat.Escapable(@char))
                        {
                            return context.AppendToComponentName(@char).TransitionTo(ParseState.ComponentName);
                        }
                        throw new FormatException(
                            string.Format(
                                "The component name character {0} cannot be escaped (position: {1}). Only {2} can be escaped.", 
                                @char, 
                                context.Position, 
                                new string(new[]
                                {
                                    StreamNameFormat.Escape, 
                                    StreamNameFormat.ComponentNameValueSeparator, 
                                    StreamNameFormat.ComponentSeparator
                                })));
                    }
                },
                {
                    ParseState.ComponentValue, (context, @char) =>
                    {
                        if (@char == StreamNameFormat.Escape)
                        {
                            return context.TransitionTo(ParseState.ComponentValueEscaping);
                        }
                        if (@char == StreamNameFormat.ComponentNameValueSeparator)
                        {
                            throw new FormatException(string.Format("The component name value separator ({0}) was not properly escaped (position: {1}).", StreamNameFormat.ComponentNameValueSeparator, context.Position));
                        }
                        if (@char == StreamNameFormat.ComponentSeparator)
                        {
                            if (context.ComponentValue.Length == 0)
                            {
                                throw new FormatException(string.Format("The component value cannot be empty (position: {0}). Please specify a value.", context.Position));
                            }
                            return context.AppendComponent().TransitionTo(ParseState.ComponentName);
                        }
                        return context.AppendToComponentValue(@char);
                    }
                },
                {
                    ParseState.ComponentValueEscaping, (context, @char) =>
                    {
                        if (StreamNameFormat.Escapable(@char))
                        {
                            return context.AppendToComponentValue(@char).TransitionTo(ParseState.ComponentValue);
                        }
                        throw new FormatException(
                            string.Format(
                                "The component value character {0} cannot be escaped (position: {1}). Only {2} can be escaped.",
                                @char,
                                context.Position,
                                new string(new[]
                                {
                                    StreamNameFormat.Escape, 
                                    StreamNameFormat.ComponentNameValueSeparator, 
                                    StreamNameFormat.ComponentSeparator
                                })));
                    }
                }
            };

        class ParseContext
        {
            public static readonly ParseContext Initial =
                new ParseContext(ParseState.Initial, new char[0], new char[0], 0, Empty);

            public readonly ParseState State;
            public readonly char[] ComponentName;
            public readonly char[] ComponentValue;
            public readonly int Position;

            readonly StreamName _parsedName;

            private ParseContext(ParseState state, char[] componentName, char[] componentValue, int position, StreamName parsedName)
            {
                State = state;
                ComponentName = componentName;
                ComponentValue = componentValue;
                Position = position;

                _parsedName = parsedName;
            }

            public ParseContext AppendToComponentName(Char @char)
            {
                var combined = new char[ComponentName.Length + 1];
                ComponentName.CopyTo(combined, 0);
                combined[ComponentName.Length] = @char;
                return new ParseContext(State, combined, ComponentValue, Position, _parsedName);
            }

            public ParseContext AppendToComponentValue(Char @char)
            {
                var combined = new char[ComponentValue.Length + 1];
                ComponentValue.CopyTo(combined, 0);
                combined[ComponentValue.Length] = @char;
                return new ParseContext(State, ComponentName, combined, Position, _parsedName);
            }

            public ParseContext TransitionTo(ParseState state)
            {
                return new ParseContext(state, ComponentName, ComponentValue, Position, _parsedName);
            }

            public ParseContext NextPosition()
            {
                return new ParseContext(State, ComponentName, ComponentValue, Position + 1, _parsedName);
            }

            public ParseContext AppendComponent()
            {
                return new ParseContext(
                    State, 
                    new char[0], 
                    new char[0], 
                    Position,
                    _parsedName.Append(new StreamNameComponent(new string(ComponentName), new string(ComponentValue))));
            }

            public StreamName Complete()
            {
                if (State != ParseState.ComponentValue)
                {
                    switch (State)
                    {
                        case ParseState.Initial:
                            throw new FormatException(string.Format("The stream name cannot be empty (position: {0}). Please specify a stream name with at least one component.", Position));
                        case ParseState.ComponentName:
                            throw new FormatException(string.Format("The stream name cannot end with a component name (position: {0}). Please specify a value for each component.", Position - 1));
                        case ParseState.ComponentNameEscaping:
                        case ParseState.ComponentValueEscaping:
                            throw new FormatException(string.Format("The stream name cannot begin or end with an escape character ({0}) (position: {1}). Please make sure that the stream name is properly escaped.", StreamNameFormat.Escape, Position - 1));
                    }
                }

                if (ComponentValue.Length == 0)
                {
                    throw new FormatException(string.Format("The component value cannot be empty (position: {0}). Please specify a value.", Position));
                }
                
                return AppendComponent()._parsedName;
            }
        }

        enum ParseState
        {
            Initial,
            ComponentName,
            ComponentValue,
            ComponentNameEscaping,
            ComponentValueEscaping
        }
    }
}