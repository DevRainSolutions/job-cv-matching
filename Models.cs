using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo
{
    public class Models
    {
    }

    public class ChartModel
    {
        public List<string> jobKeywords { get; set; }
        public List<string> cvKeywords { get; set; }
        public Metrics metrics { get; set; }
        public List<VectorSim> vector_sim { get; set; }
        public List<VectorUniqueSim> vector_unique_sim { get; set; }
        public List<JobScore> jobScores { get; set; }
        public List<CvScore> cvScores { get; set; }
    }
    public class Metrics
    {
        public double jaccard_neighbors { get; set; }
        public double jaccard_union_weight { get; set; }
        public double weighted_jaccard { get; set; }
        public double jaccard_custom { get; set; }
    }

    public class VectorSim
    {
        public string Term { get; set; }
        public string Score { get; set; }
    }

    public class VectorUniqueSim
    {
        public string Term { get; set; }
        public string Score { get; set; }
    }

    public class JobScore
    {
        public string word { get; set; }
        public double value { get; set; }
    }

    public class CvScore
    {
        public string word { get; set; }
        public double value { get; set; }
    }
}
