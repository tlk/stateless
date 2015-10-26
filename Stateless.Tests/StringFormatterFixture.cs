using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Stateless.Tests
{
    public class DotGraphFormatter : IStringFormatter
    {
        List<string> header = new List<string>();
        List<string> transitions = new List<string>();

        public DotGraphFormatter() {}

        public DotGraphFormatter(string name)
        {
            header.Add(string.Format(" label=\"{0}\";", name.Replace('"', '\'')));
            header.Add(" labelloc=top;");
            header.Add(" labeljust=left;");
        }

        public void Add(string source, string trigger, string guardMethodName, string guardMethodNamespace, string destination)
        {
            if (guardMethodNamespace.Equals("Stateless"))
            {
                transitions.Add(string.Format(" {0} -> {1} [label=\"{2}\"];",
                    source,
                    destination,
                    trigger
                ));
            }
            else
            {
                transitions.Add(string.Format(" {0} -> {1} [label=\"{2} [{3}]\"];",
                    source,
                    destination,
                    trigger,
                    guardMethodName
                ));
            }
        }

        public override string ToString()
        {
            string lines = string.Join(System.Environment.NewLine, header.Concat(transitions));
            string graph = "digraph {" + System.Environment.NewLine +
                            lines + System.Environment.NewLine +
                            "}";
            return graph;
        }
    }

    [TestFixture]
    public class StringFormatterFixture
    {
        bool IsTrue() {
            return true;
        }

        StateMachine<State, Trigger> sm;
        IStringFormatter dotgraphFormatter;

        [SetUp]
        public void Init()
        {
            sm = new StateMachine<State, Trigger>(State.A);
            dotgraphFormatter = new DotGraphFormatter();
        }

        [Test]
        public void SimpleTransition()
        {
            sm.Configure(State.A)
                .Permit(Trigger.X, State.B);

            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X\"];" + System.Environment.NewLine
                         + "}";
            Assert.AreEqual(expected, sm.ToString(dotgraphFormatter));
        }

        [Test]
        public void SimpleTransitions()
        {
            sm.Configure(State.A)
                .Permit(Trigger.X, State.B)
                .Permit(Trigger.Y, State.C);

            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X\"];" + System.Environment.NewLine
                         + " A -> C [label=\"Y\"];" + System.Environment.NewLine
                         + "}";
            Assert.AreEqual(expected, sm.ToString(dotgraphFormatter));
        }

        [Test]
        public void WhenDiscriminatedByAnonymousGuard()
        {
            sm.Configure(State.A)
                .PermitIf(Trigger.X, State.B, () => true);

            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X [<WhenDiscriminatedByAnonymousGuard>m__0]\"];" + System.Environment.NewLine
                         + "}";
            Assert.AreEqual(expected, sm.ToString(dotgraphFormatter));
        }

        [Test]
        public void WhenDiscriminatedByNamedDelegate()
        {
            sm.Configure(State.A)
                .PermitIf(Trigger.X, State.B, IsTrue);

            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X [IsTrue]\"];" + System.Environment.NewLine
                         + "}";
            Assert.AreEqual(expected, sm.ToString(dotgraphFormatter));
        }
    }
}
