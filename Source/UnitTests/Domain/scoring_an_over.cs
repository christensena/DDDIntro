using System;
using System.Linq;
using DDDIntro.Domain;
using FluentAssertions;
using Machine.Specifications;

namespace DDDIntro.UnitTests.Domain
{
    [Subject("scoring an over")]
    public class scoring_an_over
    {
        Establish context = () =>
            {
                var team1 = new Team(new Country("Australia").WithId(1)).WithId(1);
                team1.AddMember(new Player("Matthew", "Hayden").WithId(1));
                team1.AddMember(new Player("Mark", "Waugh").WithId(2));
                team1.AddMember(new Player("David", "Boon").WithId(3));
                team1.AddMember(new Player("Brad", "Haddin").WithId(4));
                bowler = new Player("Glenn", "McGrath").WithId(5);
                team1.AddMember(bowler);

                battingTeam = new Team(new Country("England").WithId(2)).WithId(2);
                battingTeam.AddMember(new Player("Mark", "Trescothick").WithId(13));
                battingTeam.AddMember(new Player("Graham", "Gooch").WithId(14));
                //team2.AddMember(new Player(""));

                var match = new Match(DateTime.Today, team1, battingTeam);
                firstInnings = match.NewInnings(battingTeam);
            };

        protected static TeamInnings firstInnings;
        protected static Over over;
        protected static Player bowler;
        protected static Team battingTeam;
    }

    public class completed_over_with_no_scoring_shots : scoring_an_over
    {
        private Because of = () =>
                                 {
                                     over = firstInnings.NewOver(bowler);

                                     var openingBatsman1 = battingTeam.Members.ElementAt(0);
                                     openingBatsman1Innings = firstInnings.CommenceBatterInnings(openingBatsman1);

                                     while (! over.IsOver())
                                        over.RecordDelivery(openingBatsman1, 0);
                                 };

        It should_be_a_maiden = () => over.IsMaiden().Should().BeTrue();

        It should_have_batter_runs_equal_to_zero = () => openingBatsman1Innings.RunsScored.Should().Be(0);

        It should_have_team_runs_equal_to_zero = () => firstInnings.GetScore().Should().Be(0);

        static BatterInnings openingBatsman1Innings;
    }

    public class completed_over_with_several_scoring_shots : scoring_an_over
    {
        Because of = () =>
                                 {
                                     over = firstInnings.NewOver(bowler);

                                     var openingBatsman1 = battingTeam.Members.ElementAt(0);
                                     var openingBatsman2 = battingTeam.Members.ElementAt(1);
                                     openingBatsman1Innings = firstInnings.CommenceBatterInnings(openingBatsman1);
                                     openingBatsmanTwoInnings = firstInnings.CommenceBatterInnings(openingBatsman2);

                                     over.RecordDelivery(openingBatsman1, 0);
                                     over.RecordDelivery(openingBatsman1, 2);
                                     over.RecordDelivery(openingBatsman1, 1);
                                     over.RecordDelivery(openingBatsman2, 0);
                                     over.RecordDelivery(openingBatsman2, 4);
                                     over.RecordDelivery(openingBatsman2, 0);
                                 };


        It should_have_runs_scored_matching_deliveries_recorded = () => over.RunsScored().Should().Be(7);

        It should_not_be_a_maiden = () => over.IsMaiden().ShouldBeFalse();

        It should_have_is_over_return_true = () => over.IsOver().ShouldBeTrue();

        It should_have_bowler_runs_conceded_matching_over_runs_scored =
            () => firstInnings.BowlingSpells.Single().GetRunsConceded().Should().Be(over.RunsScored());

        It should_have_batsman_runs_scored_matching_score_of_balls_faced = () =>
        {
            openingBatsman1Innings.RunsScored.ShouldEqual(3);
            openingBatsmanTwoInnings.RunsScored.ShouldEqual(4);
        };
    

        private static BatterInnings openingBatsman1Innings;
        private static BatterInnings openingBatsmanTwoInnings;
    }
}