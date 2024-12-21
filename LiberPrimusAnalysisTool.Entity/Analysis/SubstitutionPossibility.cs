namespace LiberPrimusAnalysisTool.Entity.Analysis;

public class SubstitutionPossibility
{
    public SubstitutionPossibility(string letter, string[] possibleSubstitutions)
    {
        Letter = letter;
        PossibleSubstitutions = new List<string>();
        PossibleSubstitutions.AddRange(possibleSubstitutions);
    }

    public string Letter { get; private set; }
    
    public List<string> PossibleSubstitutions { get; private set; }
}