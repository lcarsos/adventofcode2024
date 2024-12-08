const std = @import("std");
const Allocator = std.mem.Allocator;
const ArrayList = std.ArrayList;

fn parseList(alloc: Allocator, input: *const std.io.AnyReader) [2]ArrayList(i32) {
    var line = std.ArrayList(u8).init(alloc);
    defer line.deinit();
    var first = ArrayList(i32).init(alloc);
    var second = ArrayList(i32).init(alloc);

    const writer = line.writer();
    while (input.streamUntilDelimiter(writer, '\n', null)) {
        defer line.clearRetainingCapacity();

        var strs = std.mem.split(u8, line, "   ");
        const match: i32 = std.fmt.parseInt(i32, strs.next().?, 10) catch return .{ first, second };
        first.append(match) catch unreachable;
        second.append(std.fmt.parseInt(i32, strs.next().?, 10) catch unreachable) catch unreachable;
    }
    unreachable;
    //return .{ first, second };
}

pub fn main() !void {
    var allocator = std.heap.GeneralPurposeAllocator(.{}){};
    const alloc = allocator.allocator();

    const stdin = std.io.getStdIn();
    var buf_reader = std.io.bufferedReader(stdin.reader());
    const reader = buf_reader.reader();
    //const input: []const u8 = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3\n";
    const parsed = parseList(alloc, reader);
    defer parsed[0].deinit();
    defer parsed[1].deinit();
    std.mem.sort(i32, parsed[0].items, {}, std.sort.asc(i32));
    std.mem.sort(i32, parsed[1].items, {}, std.sort.asc(i32));

    std.debug.print("{d} {d}\n", .{parsed[0].items.len, parsed[1].items.len});
    var diff = try ArrayList(i32).initCapacity(alloc, parsed[0].items.len);
    try diff.resize(parsed[0].items.len);
    var sum: i32 = 0;
    for (parsed[0].items, parsed[1].items, 0..) |x, y, i| {
        const this_diff = y - x;
        if (this_diff < 0) {
            std.debug.print("found a negative at idx {d}\n", .{i});
        }
        diff.items[i] = this_diff;
        sum += this_diff;
    }
    std.debug.print("{d}\n", .{sum});
}
