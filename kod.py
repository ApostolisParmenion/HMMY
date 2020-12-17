import os.path
from termcolor import colored
from ortools.constraint_solver import routing_enums_pb2
from ortools.constraint_solver import pywrapcp
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

def create_data_model(Graph,ways,Route,depot,vehiclesGiven):
    """Stores the data for the problem."""
    data = {}
    flag=0
    data['distance_matrix']=[]
    dijkstra=DijkstraMain(Graph)
    nodes=list()
    for i in Graph:
        nodes.append(i[0])
    for i in Route:
        temp=[]
        for u in Route:
            if(dijkstra[i].get_distance(Graph[u][0])=="inf"):
                print(colored("Unfortunately the path is not accesible",'red'))
                return
            temp.append(dijkstra[i].get_distance(Graph[u][0]))
        data['distance_matrix'].append(temp)

    data['num_vehicles'] = vehiclesGiven
    data['depot'] = depot
    #sprint(dijkstra[0].get_distance(Graph[6][0]))
    return data

def print_solution(data, manager, routing, solution,route,ways):
    """Prints solution on console."""
    max_route_distance = 0
    for vehicle_id in range(data['num_vehicles']):
        index = routing.Start(vehicle_id)
        plan_output = 'Route for vehicle {}:\n'.format(vehicle_id)
        route_distance = 0
        while not routing.IsEnd(index):
            temp=ways[route[manager.IndexToNode(index)]]
            plan_output += ' {} -> '.format(temp[len(temp)-1])
            previous_index = index
            index = solution.Value(routing.NextVar(index))
            route_distance += routing.GetArcCostForVehicle(
                previous_index, index, vehicle_id)
        temp=ways[route[manager.IndexToNode(index)]]
        plan_output += '{}\n'.format((temp[len(temp)-1]))
        plan_output += 'Distance of the route: {}m\n'.format(route_distance)
        print(plan_output)
        max_route_distance = max(route_distance, max_route_distance)
    print('Maximum of the route distances: {}m'.format(max_route_distance))

def MakeWays():
    ways={}
    ways['Nodes']=[]
    ways['Names']=[]
    ways['OneWay']=[]
    mapPathStr="map2.osm"
    f = open(mapPathStr, "r",encoding="utf8")
    line=f.readline()
    flag=False
    flag2=False
    tempStr=""
    while(line!=""):
        oneWay=False
        waysNodes=[]
        waysNames=[]
        temp=line.split()
        if(temp[0] == "<way"):
            while(temp[0] != "</way>"):
                for k in line.split():
                    if(("id=" in k) and ("uid=" not in k)):
                        k=k.replace('id=','')
                        k=k.replace('"', '')
                        waysNodes.append(int(k))
                    elif("ref=" in k):
                        k=k.replace('ref=','')
                        k=k.replace('"', '')
                        k=k.replace('/>', '')
                        waysNodes.append(int(k)) 
                    elif("oneway" in k):
                        k=k.replace('ref=','')
                        k=k.replace('"', '')
                        oneWay=True
                    elif("name" in k ):
                        flag=True
                    elif(flag):
                        k=k.replace('v="','')
                        k=k.replace('"', '')
                        if('/>' in k):
                            k=k.replace('/>', '')
                            tempStr=tempStr+k
                            waysNames.append(tempStr)   
                            flag=False
                            flag2=True
                            tempStr=""
                        else:
                            tempStr=tempStr+k+' '
                line=f.readline()

                temp=line.split()
            if(flag2):
                ways['Nodes'].append(waysNodes)
                ways['Names'].append(waysNames)
                ways['OneWay'].append(oneWay)
                
                flag2=False
        else:
            line=f.readline()
    return ways

def MakeGraph(ways):
    Graph=[]
    ConnectetWays=[]
    for i in ways:
        distance=0
        ConnectetWays.append(i[0])
        ConnectetWays.append(distance)
        for j in i:
            if(j!=i[0]): 
                distance=distance+1
                for k in ways:
                    if(k[0]!=i[0] and k[0] not in ConnectetWays):
                        for l in k:
                            if (j==l ):#and j!="ONEWAY"):
                                ConnectetWays.append(k[0])
                                ConnectetWays.append(distance)                               
                    
        Graph.append(ConnectetWays)
        ConnectetWays=[] 
    return Graph
def main():
    ways=MakeWays()
    Graph=MakeGraph(ways['Nodes'])
    waysFound=""
    EndpointsGiven=False
    RoadFound=False
    vehiclesGiven=1
    Route=[]
    depot=0
    vehicleSet=False
    while(vehicleSet==False):
        try:
            vehiclesGiven=int(input("Give the number of vehicles (1-50): "))
            while (vehiclesGiven<0 and vehiclesGiven>50):
                vehiclesGiven=int(input("Give the number of vehicles (1-50): "))
            vehicleSet=True
        except:
            print(colored("Please enter a valid number",'red'))
    UserInput=input("Choose an action:\n 1: See all roads \n 2: Set starting road (Default is the first road on the list)\n 3: Set destination roads (At least "+ str(vehiclesGiven)+ ") (" + waysFound +")\n 4: Run!\n")
    while(True):
        RoadFound=False
        if(UserInput==str(1)):
            for i in ways['Names']:
                print(i)
        elif(UserInput==str(2)):
            while(RoadFound==False):
                roadGiven=input("Give the name of the road: ")
                for i in range(len(ways['Names'])):
                    for j in ways['Names'][i]:
                        if(j==roadGiven):
                            print(colored("Road Set!",'green'))
                            depot=i
                            RoadFound=True
                            break
                    if(RoadFound):
                        break
                if(RoadFound==False):
                    print(colored("Road name NOT Found!!",'red'))
        elif(UserInput==str(3)):
            while(RoadFound==False):
                roadGiven=input("Give the name of the road: ")
                for i in range(len(ways['Names'])):
                    for j in ways['Names'][i]:
                        if(j==roadGiven):
                            print(colored("Road Added!",'green'))
                            if(waysFound==""):
                                waysFound+= roadGiven
                            else:
                                waysFound+= ", "+roadGiven
                            Route.append(i)
                            RoadFound=True
                            EndpointsGiven=True
                            break
                    if(RoadFound):
                            break
                if(RoadFound==False):
                    print(colored("Road name NOT Found!!",'red'))
        elif(UserInput==str(4)):
            if(EndpointsGiven==False and len(Route)>=vehiclesGiven):
                print(colored(' Please Give all the information needed!', 'red'))
            else:
                break
        UserInput=input("Choose an action:\n 1: See all roads \n 2: Set starting road (Default is the first road on the list)\n 3: Set destination roads (At least "+ str(vehiclesGiven)+ ") (" + waysFound +")\n 4: Run!\n")
    
    data=create_data_model(Graph,ways['Nodes'],Route,depot,vehiclesGiven)
    for i in data['distance_matrix']:
       print (i)
    # Create the routing index manager.
    manager = pywrapcp.RoutingIndexManager(len(data['distance_matrix']),
                                           data['num_vehicles'], data['depot'])

    # Create Routing Model.
    routing = pywrapcp.RoutingModel(manager)


    # Create and register a transit callback.
    def distance_callback(from_index, to_index):
        """Returns the distance between the two nodes."""
        # Convert from routing variable Index to distance matrix NodeIndex.
        from_node = manager.IndexToNode(from_index)
        to_node = manager.IndexToNode(to_index)
        return data['distance_matrix'][from_node][to_node]

    transit_callback_index = routing.RegisterTransitCallback(distance_callback)

    # Define cost of each arc.
    routing.SetArcCostEvaluatorOfAllVehicles(transit_callback_index)

    # Add Distance constraint.
    dimension_name = 'Distance'
    routing.AddDimension(
        transit_callback_index,
        0,  # no slack
        3000,  # vehicle maximum travel distance
        True,  # start cumul to zero
        dimension_name)
    distance_dimension = routing.GetDimensionOrDie(dimension_name)
    distance_dimension.SetGlobalSpanCostCoefficient(100)

    # Setting first solution heuristic.
    search_parameters = pywrapcp.DefaultRoutingSearchParameters()
    search_parameters.first_solution_strategy = (
        routing_enums_pb2.FirstSolutionStrategy.PATH_CHEAPEST_ARC)

    # Solve the problem.
    solution = routing.SolveWithParameters(search_parameters)

    # Print solution on console.
    if solution:
        print_solution(data, manager, routing, solution,Route,ways['Names'])


if __name__ == '__main__':
    main()