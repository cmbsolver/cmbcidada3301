namespace LiberPrimusAnalysisTool.Entity.Analysis;

public class SubstitutionPossibility
{
    public SubstitutionPossibility(string letter, string[] possibleSubstitutions)
    {
        Letter = letter;
        PossibleSubstitutions = new List<string>();
        PossibleSubstitutions.AddRange(possibleSubstitutions);
        CurrentSubstitutionIndex = -1;
    }

    public string Letter { get; private set; }
    
    private List<string> PossibleSubstitutions { get; set; }
    
    public int CurrentSubstitutionIndex { get; set; }

    public SubstitutionPossibility? GetNext()
    {
        CurrentSubstitutionIndex++;
        if (CurrentSubstitutionIndex >= PossibleSubstitutions.Count)
        {
            CurrentSubstitutionIndex = -1;
            return null;
        }
        else
        {
            var nextSubstitution = new string[1] { PossibleSubstitutions[CurrentSubstitutionIndex] };
            return new SubstitutionPossibility(Letter, nextSubstitution);
        }
    }

    public SubstitutionPossibility? GetCurrent()
    {
        int currentIndex = CurrentSubstitutionIndex > -1 || CurrentSubstitutionIndex < PossibleSubstitutions.Count - 1 
            ? CurrentSubstitutionIndex : 0;
        var nextSubstitution = new string[1] { PossibleSubstitutions[currentIndex] };
        return new SubstitutionPossibility(Letter, nextSubstitution);
    }
    
    public bool HasNextSubstitution()
    {
        return CurrentSubstitutionIndex < PossibleSubstitutions.Count - 1;
    }
    
    public void Reset()
    {
        CurrentSubstitutionIndex = -1;
    }
}