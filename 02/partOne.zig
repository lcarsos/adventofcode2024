const std = @import("std");
const ArrayList = std.ArrayList;
const stdin = std.io.getStdIn();
const dbgPrt = std.debug.print;
const splitSequence = std.mem.splitSequence;

pub fn main() !void  {
    var arena = std.heap.ArenaAllocator.init(std.heap.page_allocator);
    defer arena.deinit();
    const alloc = arena.allocator();

    var a = ArrayList(u8).init(alloc);
    defer a.deinit();
    var buffered_reader = std.io.bufferedReader(stdin.reader().any());
    const buf_read = buffered_reader.reader();

    while (buf_read.streamUntilDelimiter(a.writer(), '\n', 40)): (a.shrinkRetainingCapacity(0)) {
        var iter = splitSequence(u8, a.items, " ");
        const foo = iter.next().?;
        const num: i32 = try std.fmt.parseInt(i32, foo, 10);
        dbgPrt("{s} | {d}\n", .{ a.items, num });
    } else |_| {
        dbgPrt("Reached the end of input", .{});
    }



}
