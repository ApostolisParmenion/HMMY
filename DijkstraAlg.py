import sys
from dijkstra import Graph
from dijkstra import DijkstraSPF
def DijkstraMain(graphos):
    graph = Graph()
    nodes=list()
    for i in graphos:
        nodes.append(i[0])
    k=0
    for i in graphos:
        for j in i:
            if (j<10000 and j!=i[0]):
                graph.add_edge(i[0], k, j)  
            k=j
    dijkstra=[]
    for i in range(len(nodes)):
        dijkstra.append(DijkstraSPF(graph, graphos[i][0]))
    return dijkstra