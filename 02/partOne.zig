const std = @import("std");
const ArrayList = std.ArrayList;
const stdin = std.io.getStdIn();
const dbgPrt = std.debug.print;
const splitSequence = std.mem.splitSequence;

pub fn main() !void {
    var arena = std.heap.ArenaAllocator.init(std.heap.page_allocator);
    defer arena.deinit();
    const alloc = arena.allocator();

    var a = ArrayList(u8).init(alloc);
    defer a.deinit();
    var buffered_reader = std.io.bufferedReader(stdin.reader().any());
    const buf_read = buffered_reader.reader();

    var safe_count: u32 = 0;
    while (buf_read.streamUntilDelimiter(a.writer(), '\n', 40)) : (a.shrinkRetainingCapacity(0)) {
        var iter = splitSequence(u8, a.items, " ");
        var last: i32 = try std.fmt.parseInt(i32, iter.first(), 10);
        iter.reset();
        var is_safe: bool = true;
        while (iter.next()) |current_str| {
            const current: i32 = try std.fmt.parseInt(i32, current_str, 10);
            dbgPrt("{d} {d}\n", .{ last, current });
            const diff: i32 = @intCast(@abs(current - last));
            if (diff >= 3) {
                is_safe = false;
                break;
            }
            last = current;
        }
        if (is_safe) {
            safe_count += 1;
        }
        dbgPrt("{s} | {}\n", .{ a.items, is_safe });
    } else |_| {
        dbgPrt("Reached the end of input", .{});
    }
}
