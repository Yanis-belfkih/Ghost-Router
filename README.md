# Ghost-Router
A* Algorithm included in a "virus" to achieve bypassing Anti-virus's EDR.  

classDiagram
    %% Définition des modules et interfaces
    class IEDR_Rule {
        <<interface>>
        +evaluate_state(state: AttackState) boolean
    }
    
    class StrictEDR {
        -max_gauge_threshold: int = 80
        +evaluate_state(state: AttackState) boolean
    }
    
    class LenientEDR {
        -max_gauge_threshold: int = 100
        +evaluate_state(state: AttackState) boolean
    }

    class AttackState {
        <<immutable>>
        +int current_pid
        +String attack_step
        +Tuple process_gauges
        +float g_cost
        +float h_cost
        +float f_cost
        +AttackState parent
        +generate_hash() String
    }

    class AStarSolver {
        -PriorityQueue open_set
        -HashSet closed_set
        -IEDR_Rule edr_validator
        +solve(start_state: AttackState) Generator~AttackState~
        -calculate_heuristic(state: AttackState) float
    }

    class SimulationSession {
        -AStarSolver engine
        -UI_Observer ui_listener
        +start_simulation(config: Config)
        +process_engine_yield(state: AttackState)
        +build_final_timeline(winning_state: AttackState)
    }

    class GraphCanvas {
        +draw_node(state: AttackState)
        +update_progress(percent: float)
    }

    class TimelineView {
        +render_winning_path(path: List~AttackState~)
    }

    %% Relations
    IEDR_Rule <|-- StrictEDR : Implémente
    IEDR_Rule <|-- LenientEDR : Implémente
    AStarSolver --> IEDR_Rule : Utilise
    AStarSolver ..> AttackState : Crée / Évalue
    SimulationSession --> AStarSolver : Contrôle (Asynchrone)
    SimulationSession --> GraphCanvas : Notifie (Temps réel)
    SimulationSession --> TimelineView : Notifie (Fin)
