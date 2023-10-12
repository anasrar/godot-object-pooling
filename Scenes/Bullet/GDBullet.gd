class_name GDBullet
extends Area2D

const inactive_pool_item: Theme = preload("res://Themes/InactivePoolItem/InactivePoolItem.theme")

var _speed: float = 120
var _direction: Vector2 = Vector2.RIGHT
var _panel_pool_item: Panel = Panel.new()
var _timer_destroy: Timer = Timer.new()

var active: bool = false

func _ready():
    visible = false
    monitoring = false
    monitorable = false
    set_process(false)

    _panel_pool_item.theme = GDBullet.inactive_pool_item
    (get_node("/root/Scenes/BattleField/CanvasLayer/Control/Bullets") as Control).add_child(_panel_pool_item, true)

    _timer_destroy.wait_time = 350 / _speed
    _timer_destroy.timeout.connect(func(): destroy())
    add_child(_timer_destroy)

    area_entered.connect(func(enemy: Area2D):
        (enemy as GDEnemy).destroy()
        destroy()
    )

func _process(delta):
    position += _direction * _speed * delta

func set_direction(angle: float):
    position = Vector2.ZERO
    _direction = Vector2.RIGHT.rotated(angle)

func spawn(angle: float):
    active = true
    visible = true
    monitoring = true
    monitorable = true
    set_process(true)
    get_parent().move_child(self, -1)

    _panel_pool_item.theme = null
    set_direction(angle)
    _timer_destroy.start()

func destroy():
    var status: GDEnvs.PoolingStatus = (get_node("/root/Envs") as GDEnvs).pooling

    match status:
        GDEnvs.PoolingStatus.ACTIVE:
            active = false
            visible = false
            set_deferred("monitoring", false)
            set_deferred("monitorable", false)
            set_process(false)
            get_parent().move_child(self, 0)

            _panel_pool_item.theme = GDBullet.inactive_pool_item
            _timer_destroy.stop()
        GDEnvs.PoolingStatus.INACTIVE:
            _panel_pool_item.queue_free()
            queue_free()
