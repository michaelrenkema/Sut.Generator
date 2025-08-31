using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example9>]
public partial class Test9
{
  [Test]
  [Arguments(1, 1, false, true)]
  [Arguments(1, 1, true, false)]
  [Arguments(1, 2, false, false)]
  public async Task Success(int inputId, int expectedId, bool isCancelled, bool expectedResult)
  {
    var command = new Command(inputId);
    var cancellationTokenSource = new CancellationTokenSource();
    var cancellationToken = cancellationTokenSource.Token;

    if (isCancelled)
      cancellationTokenSource.Cancel();

    var sut = this.Sut
      .With_Dependency6_Evaluate((f) => f(command, cancellationToken), (t) => t == cancellationToken, expectedResult)
      .Build();

    var result = sut.Evaluate(command, cancellationToken);

    await Assert.That(result).IsEqualTo(expectedResult);
  }
}
