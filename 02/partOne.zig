const std = @import("std");
const ArrayList = std.ArrayList;
const stdin = std.io.getStdIn();
const dbgPrt = std.debug.print;
const splitSequence = std.mem.splitSequence;
const parseInt = std.fmt.parseInt;

const V4 = @Vector(4, i8);

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
        const first = V4{
            try parseInt(i8, iter.next().?, 10),
            try parseInt(i8, iter.next().?, 10),
            try parseInt(i8, iter.next().?, 10),
            try parseInt(i8, iter.next().?, 10)
        };
        const second: V4 = @shuffle(
            i8,
            first,
            V4{ 0, 0, 0, try parseInt(i8, iter.next().?, 10) },
            V4{ 1, 2, 3, -4 }
        );
        const sub: @Vector(4, i8) = second - first;
        const decreasing = @reduce(.And, sub < @as(V4, @splat(0)));
        const dec_slowly = @reduce(.Or, sub < @as(V4, @splat(-3)));
        const increasing = @reduce(.And, sub > @as(V4, @splat(0)));
        const inc_slowly = @reduce(.Or, sub > @as(V4, @splat(3)));
        const slowly = !(dec_slowly or inc_slowly);
        const uniform = (decreasing and !increasing) or (!decreasing and increasing);
        const safe = uniform and slowly;
        const counter: u32 = @intFromBool(safe);
        safe_count += @intFromBool(safe);

        dbgPrt("{s}: {any}\tv{any}\t^{any}\t={any}\t~{any}: {d}\n", .{ a.items, sub, decreasing, increasing, uniform, slowly, counter });
    } else |_| {
        dbgPrt("Reached the end of input\n", .{});
    }
    const stdout = std.io.getStdOut();
    var output = ArrayList(u8).init(alloc);
    try std.fmt.format(output.writer(), "{d}\n", .{ safe_count });
    _ = try stdout.write(output.items);
}
