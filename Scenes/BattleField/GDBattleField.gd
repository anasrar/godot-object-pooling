class_name GDBattleField
extends Node2D

@export var path_pool_enemies: NodePath
@export var grid_bullets: Control
@export var grid_enemies: Control

var _timer_spawn_enemy: Timer = Timer.new()

func _ready():
    var pool_enemies: GDPool = get_node(path_pool_enemies) as GDPool;

    _timer_spawn_enemy.wait_time = lerp(0.2, 1.0, randf())
    _timer_spawn_enemy.timeout.connect(func():
        var enemy: GDEnemy = pool_enemies.get_item() as GDEnemy
        enemy.spawn()
        _timer_spawn_enemy.wait_time = lerp(0.2, 1.0, randf())
        _timer_spawn_enemy.start()
    )
    add_child(_timer_spawn_enemy, true)

    _timer_spawn_enemy.start()

func _on_bullets_child_order_changed():
    const size: float = 24
    const gap: float = 8
    const origin: Vector2 = Vector2(377, 82);
    var panels = grid_bullets.get_children()
    for i in range(panels.size()):
        var panel: Panel = panels[i] as Panel
        panel.size = Vector2(size, size)
        var column = i % 5
        var row: int = int(i / 5.0)
        panel.position = origin + Vector2((size * column) + (gap * column), (size * row) + (gap * row))

func _on_enemies_child_order_changed():
    const size: int = 24
    const gap: int = 8
    const origin: Vector2 = Vector2(323 - size, 82);
    var panels = grid_enemies.get_children()
    for i in range(panels.size()):
        var panel: Panel = panels[i] as Panel
        panel.size = Vector2(size, size)
        var column: int = i % 5
        var row: int = int(i / 5.0)
        panel.position = origin + Vector2(-((size * column) + (gap * column)), (size * row) + (gap * row))
